using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Security;
using Abp.UI;
using Fanap.DataLabeling.Authentication;
using Fanap.DataLabeling.Authentication.JwtBearer;
using Fanap.DataLabeling.Authorization;
using Fanap.DataLabeling.Authorization.Roles;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Clients.Pod;
using Fanap.DataLabeling.Clients.Pod.Dtos;
using Fanap.DataLabeling.Configuration;
using Fanap.DataLabeling.Controllers;
using Fanap.DataLabeling.Jwt;
using Fanap.DataLabeling.Models.TokenAuth;
using Fanap.DataLabeling.MultiTenancy;
using Fanap.DataLabeling.Pod;
using Fanap.DataLabeling.Pod.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Web.Host.Controllers
{
    [AllowAnonymous]
    [Route("pod/authentication")]
    public class PodAuthenticationController : DataLabelingControllerBase
    {
        private readonly IAccessTokenManager accessTokenManager;
        private readonly TokenAuthConfiguration tokenAuthConfiguration;
        private readonly AbpLoginResultTypeHelper abpLoginResultTypeHelper;
        private readonly LogInManager logInManager;
        private readonly IRepository<ExternalToken> externalTokensRepo;
        private readonly UserManager userManager;
        private readonly IRepository<User, long> userRepo;
        private readonly IRepository<StaticUser> staticUserRepo;
        private readonly IPodClient _service;
        private readonly IJwtCreator _jwtCreator;
        public PodAuthenticationController(
            IAccessTokenManager accessTokenManager,
            TokenAuthConfiguration tokenAuthConfiguration,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            LogInManager logInManager,
            IRepository<ExternalToken> externalTokensRepo,
            UserManager userManager,
            IRepository<User, long> userRepo,
            IRepository<StaticUser> staticUserRepo,
            IPodClient service,
            IJwtCreator jwtCreator)
        {
            this.accessTokenManager = accessTokenManager;
            this.tokenAuthConfiguration = tokenAuthConfiguration;
            this.abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            this.logInManager = logInManager;
            this.externalTokensRepo = externalTokensRepo;
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.staticUserRepo = staticUserRepo;
            _service = service;
            _jwtCreator = jwtCreator;
        }

        [HttpGet("")]
        public IActionResult CallPodAuthentication()
        {
            var uri = SettingManager.GetSettingValue(AppSettingNames.PodUri);
            var clientId = SettingManager.GetSettingValue(AppSettingNames.PodClientId);
            var callbackUrl = WebUtility.UrlEncode($"{Request.Scheme}://{Request.Host}/pod/authentication/callback");
            var variables =
                $"/authorize/?client_id={clientId}&response_type=code&redirect_uri={callbackUrl}&scope=profile";
            return Redirect($"{uri}{variables}");
        }

        [HttpGet("callback")]
        public async Task Callback(string code)
        {
            try
            {
                using (AbpSession.Use(1, null))
                {
                    CurrentUnitOfWork.SetTenantId(1);
                    Logger.Info($"{nameof(code)} : {code}");

                    var callbackUrl = $"{Request.Scheme}://{Request.Host}/pod/authentication/callback";
                    var podToken = await _service.GetTokenAsync(callbackUrl, code);

                    Logger.Info($"{nameof(podToken.AccessToken)} : {podToken.AccessToken}");

                    var profileInfo = await _service.GetUserProfileAsync(podToken.AccessToken);
                    var ssoId = profileInfo.ssoId;

                    Logger.Info($"{nameof(profileInfo)} : {profileInfo}");

                    var user = await userRepo.GetAll().SingleOrDefaultAsync(ff => ff.UserName == profileInfo.Username);

                    Logger.Info($"{nameof(user)} : {user}");

                    if (user == null)
                    {
                        user = await ImportUserFromPodAsync(profileInfo);
                        CurrentUnitOfWork.SaveChanges();
                    }
                    else
                    {
                        await EditUserProfileBasedOnPod(user, profileInfo);
                        CurrentUnitOfWork.SaveChanges();
                    }

                    var phone = GetIndividualUserPhoneAsync(profileInfo);

                    externalTokensRepo.Insert(new ExternalToken
                    {
                        UserId = user.Id,
                        AccessToken = podToken.AccessToken,
                        CreationTime = DateTime.UtcNow,
                        Provider = "Pod",
                        RefreshToken = podToken.RefreshToken
                    });

                    var loginResult = await GetLoginResultAsync(
                               user.UserName,
                               "123qwe@QWE",
                               "Default"
                           );

                    var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                    var encryptedAccessToken = GetEncryptedAccessToken(accessToken);

                    var redurectUrl = SettingManager.GetSettingValue(AppSettingNames.AuthenticationRedirectUrl);
                    var base64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(accessToken));
                    Response.Redirect($"{redurectUrl}/{user.Id}/?token={base64Token}");
                }

            }
            catch (Exception e)
            {
                Logger.Error("Error in authentication callback", e);

                throw new UserFriendlyException("There was an error in authentication callback. Please take a look at system log to get more information");
            }
        }


        [HttpGet("profile")]
        [AbpMvcAuthorize]
        public async Task<UserProfileInfo> Profile()
        {
            var podToken = await accessTokenManager.GetCurrentAccessToken();
            var profileInfo = await _service.GetUserProfileAsync(podToken);

            return profileInfo;
        }


        [HttpPost("profile/edit")]
        [AbpMvcAuthorize]
        public async Task<UserProfileInfo> EditProfile(UserProfileInfo newProfile)
        {
            var podToken = await accessTokenManager.GetCurrentAccessToken();
            var profileInfo = await _service.GetUserProfileAsync(podToken);
            var user = userRepo.Get(AbpSession.UserId.Value);

            // edit pod profile
            var editedProfile = await _service.EditUserProfileAsync(podToken, newProfile);
            
            await EditUserProfileBasedOnPod(user, newProfile);
            await CurrentUnitOfWork.SaveChangesAsync();

            return profileInfo;
        }

        private async Task EditUserProfileBasedOnPod(User user, UserProfileInfo profileInfo)
        {
            user.Name = profileInfo.FirstName;
            user.Surname = profileInfo.LastName;
            user.PodUserId = profileInfo.UserId;
            user.UserName = profileInfo.Username;
            user.PhoneNumber = profileInfo.CellphoneNumber;
            user.ProfileImage = profileInfo.ProfileImage;
            await userRepo.UpdateAsync(user);
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: tokenAuthConfiguration.Issuer,
                audience: tokenAuthConfiguration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? tokenAuthConfiguration.Expiration),
                signingCredentials: tokenAuthConfiguration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                });

            return claims;
        }
        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private async Task<User> ImportUserFromPodAsync(UserProfileInfo profileInfo)
        {
            var user = new User
            {
                EmailAddress = string.IsNullOrEmpty(profileInfo.Email) ? $"{profileInfo.Username}@pod.login" : profileInfo.Email,
                Name = profileInfo.FirstName,
                Surname = profileInfo.LastName,
                PodUserId = profileInfo.UserId,
                UserName = profileInfo.Username,
                PhoneNumber = profileInfo.CellphoneNumber,
                ProfileImage = profileInfo.ProfileImage,
                IsPhoneNumberConfirmed = true,
                IsEmailConfirmed = false,
                IsImported = true,
                TenantId = 1
            };

            var res = await userManager.CreateAsync(user, "123qwe@QWE");
            if (!res.Succeeded)
            {
                Logger.Error(string.Join(',', res.Errors.Select(ff => ff.Description)));
                throw new UserFriendlyException("Importing user failed");
            }

            await AssignStaticRoles(profileInfo, user);
            return user;
        }

        private async Task AssignStaticRoles(UserProfileInfo profileInfo, User user)
        {
            var isAdminUser = staticUserRepo.GetAll().Count(ff => ff.IsAdmin == true && ff.PodUserName == profileInfo.Username) > 0;
            await userManager.AddToRoleAsync(user, StaticRoleNames.Tenants.Admin);
        }

        private void LogErrorAndGetRefId(string errorMessage, string exceptionType, int? userId)
        {
            var exceptionInfo = new
            {
                UserId = userId,
                CurrentPageUrl = Response.HttpContext.Request.Path,
                ExceptionType = exceptionType.ToString(),
                Message = errorMessage
            };

            Logger.Error(JsonConvert.SerializeObject(exceptionInfo));
        }

        private static string GetIndividualUserPhoneAsync(UserProfileInfo userInfo)
        {
            return userInfo.CellphoneNumber;
        }

    }


}

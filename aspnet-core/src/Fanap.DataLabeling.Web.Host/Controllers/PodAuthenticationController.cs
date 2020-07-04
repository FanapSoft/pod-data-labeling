using Abp.Domain.Repositories;
using Abp.UI;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Clients.Pod;
using Fanap.DataLabeling.Clients.Pod.Dtos;
using Fanap.DataLabeling.Configuration;
using Fanap.DataLabeling.Controllers;
using Fanap.DataLabeling.Jwt;
using Fanap.DataLabeling.Pod;
using Fanap.DataLabeling.Pod.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Web.Host.Controllers
{
    [AllowAnonymous]
    [Route("pod/authentication")]
    public class PodAuthenticationController : DataLabelingControllerBase
    {
        private readonly IRepository<ExternalToken> externalTokensRepo;
        private readonly UserManager userManager;
        private readonly IRepository<User, long> userRepo;
        private readonly IPodClient _service;
        private readonly IJwtCreator _jwtCreator;
        private readonly IPodClient _podClient;
        public PodAuthenticationController(
            IRepository<ExternalToken> externalTokensRepo,
            UserManager userManager,
            IRepository<User, long> userRepo,
            IPodClient service,
            IJwtCreator jwtCreator,
            IPodClient podClient)
        {
            this.externalTokensRepo = externalTokensRepo;
            this.userManager = userManager;
            this.userRepo = userRepo;
            _service = service;
            _jwtCreator = jwtCreator;
            _podClient = podClient;
        }

        [HttpGet("")]
        public IActionResult CallPodAuthentication()
        {
            var uri = SettingManager.GetSettingValue(AppSettingNames.PodUri);
            var clientId = SettingManager.GetSettingValue(AppSettingNames.PodClientId);
            var callbackUrl = WebUtility.UrlEncode($"{Request.Scheme}://{Request.Host}/pod/callback");
            var variables =
                $"/authorize/?client_id={clientId}&response_type=code&redirect_uri={callbackUrl}&scope=profile";
            return Redirect($"{uri}{variables}");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            try
            {
                Logger.Info($"{nameof(code)} : {code}");

                var callbackUrl = $"{Request.Scheme}://{Request.Host}/pod/callback";
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
                }

                var phone = GetIndividualUserPhoneAsync(profileInfo);

                var loginResponse = new LoginResponseDto
                {
                    FristName = user.Name + "",
                    LastName = user.Surname + "",
                    Username = user.UserName,
                    CellphoneNumber = user.PhoneNumber,
                    IndividualUserPhone = phone,
                    Token = _jwtCreator.Create(user, podToken.AccessToken, ssoId)
                };

                externalTokensRepo.Insert(new ExternalToken
                {
                    UserId = user.Id,
                    AccessToken = podToken.AccessToken,
                    CreationTime = DateTime.UtcNow,
                    Provider = "Pod",
                    RefreshToken = podToken.RefreshToken
                });

                var base64Token = Base64Uilities.Base64Encode(JsonConvert.SerializeObject(loginResponse));
                ViewData["Result"] = base64Token;
                return View();
            }
            catch (Exception e)
            {
                Logger.Error("Error in authentication callback", e);

                throw new UserFriendlyException("There was an error in authentication callback. Please take a look at system log to get more information");
            }
        }

        private async Task<User> ImportUserFromPodAsync(UserProfileInfo profileInfo)
        {
            var user = new User
            {
                EmailAddress = profileInfo.Email,
                Name = profileInfo.FirstName,
                Surname = profileInfo.LastName,
                PodUserId = profileInfo.UserId,
                UserName = profileInfo.Username,
                PhoneNumber = profileInfo.CellphoneNumber,
                IsPhoneNumberConfirmed = true,
                IsImported = true,
            };

            var res = await userManager.CreateAsync(user);
            if (!res.Succeeded)
            {
                Logger.Error(string.Join(',', res.Errors.Select(ff => ff.Description)));
                throw new UserFriendlyException("Importing user failed");
            }
            return user;
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

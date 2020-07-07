using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Abp.Configuration;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Configuration;
using Fanap.DataLabeling.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace FanapPlus.Torange.Core.Jwt
{
    public class JwtCreator : IJwtCreator
    {
        private readonly ISettingManager settingManager;

        public JwtCreator(ISettingManager settingManager)
        {
            this.settingManager = settingManager;
        }
        public string Create(User user, string accessToken , string ssoId)
        {
            var key = settingManager.GetSettingValue(AppSettingNames.AuthenticationSecretKey);
            var issuer = settingManager.GetSettingValue(AppSettingNames.AuthenticationIssuer);
            var audience = settingManager.GetSettingValue(AppSettingNames.AuthenticationAudience);
            var lifeTime = settingManager.GetSettingValue(AppSettingNames.AuthenticationLifeTime);
            
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            

            var tokeOptions = new JwtSecurityToken(
                issuer,
                audience,
                new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) ,
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("ExternalAccessToken", accessToken),
                        new Claim("SsoId", ssoId)
                    },
                expires: DateTime.UtcNow.Add(TimeSpan.Parse(lifeTime)),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }


        public TokenValidationParameters GetTokenValidationParameters()
        {
            var key = settingManager.GetSettingValue(AppSettingNames.AuthenticationSecretKey);
            var issuer = settingManager.GetSettingValue(AppSettingNames.AuthenticationIssuer);
            var audience = settingManager.GetSettingValue(AppSettingNames.AuthenticationAudience);
            var clockSkew = settingManager.GetSettingValue(AppSettingNames.AuthenticationClockSkew);

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Parse(clockSkew),
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        }


    }
}
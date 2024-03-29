﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Fanap.DataLabeling.Authentication;
using Fanap.DataLabeling.Clients.Pod.Dtos;
using Fanap.DataLabeling.Clients.Pod.Responses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Abp.Configuration;
using Fanap.DataLabeling.Configuration;
using Abp.UI;
using System.Runtime.Loader;
using Fanap.DataLabeling.Pod.Dtos;
using System.Security.Cryptography;
using System.Web;
using Fanap.DataLabeling.Pod;
using System.Net;
using Fanap.DataLabeling.Credit;
using Fanap.DataLabeling.DataSets;
using Abp.Runtime.Session;

namespace Fanap.DataLabeling.Clients.Pod
{
    public class PodClient : IPodClient
    {
        public const string PodHttpClientName = "PodHttpClient";
        private readonly ISettingManager settingManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly TransactionsAppService _transactionsAppService;

        public PodClient(
            ISettingManager settingManager,
            IHttpClientFactory clientFactory,
            IMapper mapper,
            TransactionsAppService transactionsAppService)
        {
            this.settingManager = settingManager;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _transactionsAppService = transactionsAppService;
        }

        private HttpClient CreateClient()
        {
            return _clientFactory.CreateClient(PodHttpClientName);
        }

        public async Task<string> GenerateApiKey(string podId, string apiToken)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);
            var client = CreateClient();
            var url = $"{address}/nzh/biz/generateApiKey/";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("id", podId)
                };
                httpRequest.Content = new FormUrlEncodedContent(parameters);
                httpRequest.Headers.Add("_token_", new List<string>() { apiToken });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });
                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "سرویس تولید apikey");

                var result = JsonConvert.DeserializeObject<PodResult<GenerateApiKeyResult>>(body);
                return result.Result.ApiKey;
            }
        }

        public async Task<Token> RefreshTokenAsync(string refreshToken)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodUri);
            var clientId = settingManager.GetSettingValue(AppSettingNames.PodClientId);
            var clientSecret = settingManager.GetSettingValue(AppSettingNames.PodClientSecret);

            var client = CreateClient();
            var url = $"{address}/token";

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("refresh_token", refreshToken)
                };

                httpRequest.Content = new FormUrlEncodedContent(parameters);
                httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "احراز هویت");

                var response = JsonConvert.DeserializeObject<Token>(body);
                return response;
            }
        }

        public async Task<Token> GetTokenAsync(string callbackUrl, string code)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodUri);
            var clientId = settingManager.GetSettingValue(AppSettingNames.PodClientId);
            var clientSecret = settingManager.GetSettingValue(AppSettingNames.PodClientSecret);
            var client = CreateClient();
            var url = $"{address}/token/";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", callbackUrl),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                };

                httpRequest.Content = new FormUrlEncodedContent(parameters);
                httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    //Handle errors
                    throw new UserFriendlyException("Authentication service failed");
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<Token>(body);
                    return response;
                }
            }
        }

        public async Task<bool> IsBusinessAccountAsync(string token)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);
            var client = CreateClient();
            var url = $"{address}/nzh/getUserBusiness/";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {

                httpRequest.Headers.Add("_token_", new List<string>() { token });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "دریافت اطلاعات کسب و کار");

                var result = JsonConvert.DeserializeObject<PodResult<UserAndBusiness>>(body);
                return result.Result != null;
            }
        }

        public async Task<UserProfileInfo> GetUserProfileAsync(string podAccessToken)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);
            var clientSecret = settingManager.GetSettingValue(AppSettingNames.PodClientSecret);
            var clientId = settingManager.GetSettingValue(AppSettingNames.PodClientId);

            var client = CreateClient();
            var url = $"{address}/nzh/getUserProfile/";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                };

                httpRequest.Content = new FormUrlEncodedContent(parameters);
                httpRequest.Headers.Add("_token_", new List<string>() { podAccessToken });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "سرویس احراز هویت");

                var result = JsonConvert.DeserializeObject<PodResult<UserProfileInfo>>(body);
                return result.Result;
            }

        }

        public async Task<ContactDto> AddContactAsync(string ownerAccessToken, string userName)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);

            var client = CreateClient();
            var url = $"{address}/nzh/addContacts";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("uniqueId", DateTime.Now.Ticks.ToString()),
                    new KeyValuePair<string, string>("ownerId", ""),
                    new KeyValuePair<string, string>("firstName", ""),
                    new KeyValuePair<string, string>("lastName", ""),
                    new KeyValuePair<string, string>("cellphoneNumber", ""),
                    new KeyValuePair<string, string>("email", ""),
                    //new KeyValuePair<string, string>("typeCode", null),
                };

                httpRequest.Content = new FormUrlEncodedContent(parameters);
                httpRequest.Headers.Add("_token_", new List<string>() { ownerAccessToken });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "سرویس افزودن مخاطب");

                var result = JsonConvert.DeserializeObject<PodResult<ContactDto[]>>(body);
                return result.Result.First();
            }
        }

        public async Task<HandshakeDto> Handshake(string token)
        {
            var client = CreateClient();
            var url = $"https://accounts.pod.ir/handshake/users";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("keyAlgorithm", "RSA")
                };

                httpRequest.Content = new FormUrlEncodedContent(parameters);
                httpRequest.Headers.Add($"Authorization", $"Bearer {token}");

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "سرویس handshake");

                var result = JsonConvert.DeserializeObject<HandshakeDto>(body);
                return result;

            }
        }
        public static byte[] HashAndSignBytes(byte[] DataToSign, string key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportParameters(new RSAParameters()
                {
                    Modulus = Encoding.ASCII.GetBytes(key)
                });
                // Hash and sign the data. Pass a new instance of SHA256
                // to specify the hashing algorithm.
                return RSAalg.SignHash(DataToSign, HashAlgorithmName.SHA256.Name);
            }
            catch (CryptographicException e)
            {
                return null;
            }
        }

        public async Task<PodResult<TransferToContact>> TransferFundToContact(long userId,string contactId, BalanceOutput balance)
        {
            var amount = Convert.ToDecimal(balance.Total);          
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);
            var apiToken = settingManager.GetSettingValue(AppSettingNames.PodApiToken);
            var client = CreateClient();
            var url = $"{address}/nzh/transferToContact/?contactId={contactId}&amount={(int)amount}";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {

                httpRequest.Headers.Add("_token_", new List<string>() { apiToken });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "سرویس TransferToContact");

                _transactionsAppService.UpdateTransferedCreditStatus(userId);

                var result = JsonConvert.DeserializeObject<PodResult<TransferToContact>>(body);
                return result;
            }
        }

        public async Task<PodResult> TransferFundToContactWithSign(string token, string contactId, decimal amount)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);
            var apiToken = settingManager.GetSettingValue(AppSettingNames.PodApiToken);
            var client = CreateClient();
            var unixTimestamp = DateTime.UtcNow.ToUnixtime();
            var handshake = await Handshake(token);

            var signRaw = @$"timestamp: {unixTimestamp}
userid: {handshake.User.Id}
contactid: {contactId}
amount: {(int)amount}";

            byte[] originalData = ASCIIEncoding.UTF8.GetBytes(signRaw);

            // Hash and sign the data.
            var signedData = HashAndSignBytes(originalData, handshake.PrivateKey);
            var sign = WebUtility.UrlEncode(Convert.ToBase64String(signedData));
            var url = $"{address}/nzh/transferToContactWithSign/?contactId={contactId}&amount={(int)amount}&timestamp={unixTimestamp}&sign={sign}&keyId={handshake.KeyId}";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {

                httpRequest.Headers.Add("_token_", new List<string>() { token });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "سرویس TransferToContact");

                var result = JsonConvert.DeserializeObject<PodResult>(body);
                return result;
            }
        }

        public async Task<PodResult> ConfirmTransferFundToContact(string phoneNumber, string code)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);
            var apiToken = settingManager.GetSettingValue(AppSettingNames.PodApiToken);
            var client = CreateClient();

            var url = $"{address}/nzh/biz/confirmTransferToContact";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("cellphoneNumber", phoneNumber),
                    new KeyValuePair<string, string>("code", code),
                };

                httpRequest.Content = new FormUrlEncodedContent(parameters);
                httpRequest.Headers.Add("_token_", new List<string>() { apiToken });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "سرویس ConfirmTransferToContact");

                var result = JsonConvert.DeserializeObject<PodResult>(body);
                return result;
            }
        }

        public async Task<UserProfileInfo> EditUserProfileAsync(string podAccessToken, UserProfileInfo profile)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);
            var clientSecret = settingManager.GetSettingValue(AppSettingNames.PodClientSecret);
            var clientId = settingManager.GetSettingValue(AppSettingNames.PodClientId);

            var client = CreateClient();
            var url = $"{address}/nzh/editProfileWithConfirmation/";
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("firstName", profile.FirstName),
                    new KeyValuePair<string, string>("lastName", profile.LastName),
                    //new KeyValuePair<string, string>("cellphoneNumber", profile.CellphoneNumber),
                    new KeyValuePair<string, string>("gender", profile.Gender),
                    new KeyValuePair<string, string>("profileImage", profile.ProfileImage),
                };

                httpRequest.Content = new FormUrlEncodedContent(parameters);
                httpRequest.Headers.Add("_token_", new List<string>() { podAccessToken });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "سرویس ویرایش پروفایل");

                var result = JsonConvert.DeserializeObject<PodResult<UserProfileInfo>>(body);
                return result.Result;
            }

        }

        private void EnsureSuccessfulResponse(HttpResponseMessage httpResponse, string body, string serviceName)
        {

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new UserFriendlyException($"Error in pod api ({serviceName}) call {httpResponse.Content.ReadAsStringAsync().Result}");
            }

            var result = JsonConvert.DeserializeObject<PodResult>(body);

            if (!result.HasError)
                return;

            throw new UserFriendlyException($"Error in pod api ({serviceName}) call {httpResponse.Content.ReadAsStringAsync().Result}");

        }

        private string BuildApiUrl(string relative)
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);

            return $"{address}/{relative}";
        }

        public async Task<GetBusinessApiToken> GetBusinessApiTokenAsync(long businessId)
        {
            var token = settingManager.GetSettingValue(AppSettingNames.PodApiToken);
            var client = CreateClient();
            var url = BuildApiUrl($"nzh/biz/getApiTokenForCreatedBusiness/?businessId={businessId}");

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {
                httpRequest.Headers.Add("_token_", new List<string>() { token });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, "دریافت توکن کسب‌وکار");

                var result = JsonConvert.DeserializeObject<PodResult<GetBusinessApiToken>>(body);
                return result.Result;
            }
        }

        public async Task<GetOttResult> GetOttAsync()
        {
            var address = settingManager.GetSettingValue(AppSettingNames.PodApiBaseAddress);
            var token = settingManager.GetSettingValue(AppSettingNames.PodApiToken);
            var client = CreateClient();

            var url = $"{address}/nzh/ott";

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {
                httpRequest.Headers.Add("_token_", new List<string>() { token });
                httpRequest.Headers.Add("_token_issuer_", new List<string>() { "1" });

                var parameters = new List<KeyValuePair<string, string>>
                {
                };

                httpRequest.Content = new FormUrlEncodedContent(parameters);

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();

                EnsureSuccessfulResponse(httpResponse, body, serviceName: "دریافت ott");

                var result = JsonConvert.DeserializeObject<PodResult>(body);
                return new GetOttResult
                {
                    Ott = result.Ott
                };
            }
        }

        public async Task<PodWalletCreditDto> GetWalletCredit(string accessToken)
        {

            var client = CreateClient();
            var url = BuildApiUrl("nzh/getCredit?currencyCode=IRR&wallet=PODLAND_WALLET");

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
            {
                httpRequest.Headers.Add("_token_", accessToken);
                httpRequest.Headers.Add("_token_issuer_", "1");

                var httpResponse = await client.SendAsync(httpRequest);

                var body = await httpResponse.Content.ReadAsStringAsync();
                EnsureSuccessfulResponse(httpResponse, body, "سرویس دریافت موجودی کیف پول");

                var podWalletCreditResponse = JsonConvert.DeserializeObject<PodResult<PodWalletCreditResponse>>(body);

                var result = new PodWalletCreditDto(podWalletCreditResponse.Result);

                return result;
            }
        }

    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Fanap.DataLabeling.Clients.Pod.Dtos;
using Fanap.DataLabeling.Clients.Pod.Responses;
using Fanap.DataLabeling.Pod.Dtos;

namespace Fanap.DataLabeling.Clients.Pod
{
    public interface IPodClient: ITransientDependency
    {
        Task<TransferFundToContactDto> TransferFundToContact(string contactId, decimal amount);
        Task<ContactDto> AddContactAsync(string ownerAccessToken, string userName);
        Task<UserProfileInfo> EditUserProfileAsync(string podAccessToken, UserProfileInfo profile);
        Task<PodWalletCreditDto> GetWalletCredit(string accessToken);
        Task<Token> GetTokenAsync(string calllBackurl, string code);
        Task<Token> RefreshTokenAsync(string refreshToken);
        Task<UserProfileInfo> GetUserProfileAsync(string podTokenAccessToken);
        Task<bool> IsBusinessAccountAsync(string token);
        Task<string> GenerateApiKey(string podId, string apiToken);

    }
}
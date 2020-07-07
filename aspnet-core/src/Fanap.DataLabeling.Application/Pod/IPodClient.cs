using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Fanap.DataLabeling.Clients.Pod.Dtos;
using Fanap.DataLabeling.Clients.Pod.Responses;

namespace Fanap.DataLabeling.Clients.Pod
{
    public interface IPodClient: ITransientDependency
    {
        Task<Token> GetTokenAsync(string calllBackurl, string code);
        Task<Token> RefreshTokenAsync(string refreshToken);
        Task<UserProfileInfo> GetUserProfileAsync(string podTokenAccessToken);
        Task<bool> IsBusinessAccountAsync(string token);
        Task<string> GenerateApiKey(string podId, string apiToken);

    }
}
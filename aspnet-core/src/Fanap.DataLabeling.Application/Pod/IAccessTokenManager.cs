using Abp.Dependency;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Authentication
{
    public interface IAccessTokenManager: ITransientDependency
    {
        Task<string> GetCurrentAccessToken();
        Task RefreshTokenAsync();
        Task RefreshTokenAsync(long userId);

    }

}
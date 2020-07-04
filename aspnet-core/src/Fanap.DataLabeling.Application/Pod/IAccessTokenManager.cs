using Abp.Dependency;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Authentication
{
    // ToDo - Restructure
    public interface IAccessTokenManagerBase: ITransientDependency
    {
        Task<string> GetCurrentAccessToken();
    }

    public interface IAccessTokenManager: IAccessTokenManagerBase
    {
        Task RefreshTokenAsync();
    }
}
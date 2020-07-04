using System.Threading.Tasks;
using Abp.Application.Services;
using Fanap.DataLabeling.Authorization.Accounts.Dto;

namespace Fanap.DataLabeling.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}

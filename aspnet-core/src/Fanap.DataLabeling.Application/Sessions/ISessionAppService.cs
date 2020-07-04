using System.Threading.Tasks;
using Abp.Application.Services;
using Fanap.DataLabeling.Sessions.Dto;

namespace Fanap.DataLabeling.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}

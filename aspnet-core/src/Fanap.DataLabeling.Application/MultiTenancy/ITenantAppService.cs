using Abp.Application.Services;
using Fanap.DataLabeling.MultiTenancy.Dto;

namespace Fanap.DataLabeling.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}


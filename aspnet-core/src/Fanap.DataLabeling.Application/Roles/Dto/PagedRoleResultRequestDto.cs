using Abp.Application.Services.Dto;

namespace Fanap.DataLabeling.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}


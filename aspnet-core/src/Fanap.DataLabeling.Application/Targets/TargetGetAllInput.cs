using Abp.Application.Services.Dto;

namespace Fanap.DataLabeling.Targets
{
    public class TargetGetAllInput : PagedAndSortedResultRequestDto
    {
        public long OwnerId { get; set; }
    }
}

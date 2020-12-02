using Abp.Application.Services.Dto;
using System;

namespace Fanap.DataLabeling.Targets
{
    public class TargetGetAllInput : PagedAndSortedResultRequestDto
    {
        public long? OwnerId { get; set; }
        public Guid? DataSetId { get; set; }
    }
}

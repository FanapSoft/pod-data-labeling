using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;

namespace Fanap.DataLabeling.Targets
{

    public class TargetDefinitionGetAllInput: PagedAndSortedResultRequestDto
    {
        public Guid? DataSetId { get; set; }
        public long? OwnerId { get; set; }
    }
    public interface ITargetsAppService: IAsyncCrudAppService<TargetDefinitionDto, Guid, TargetDefinitionGetAllInput>
    { 
    
    }
}

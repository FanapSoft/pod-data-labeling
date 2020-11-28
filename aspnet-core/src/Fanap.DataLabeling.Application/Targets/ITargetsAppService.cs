using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;

namespace Fanap.DataLabeling.Targets
{

    public class TargetDefinitionGetAllInput: PagedAndSortedResultRequestDto
    {
        public Guid? DataSetId { get; set; }
    }
    public interface ITargetDefinitionsAppService: IAsyncCrudAppService<TargetDefinitionDto, Guid, TargetDefinitionGetAllInput>
    { 
    
    }
}

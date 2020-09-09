using Abp.Application.Services;
using System;

namespace Fanap.DataLabeling.Targets
{
    public interface ITargetsAppService: IAsyncCrudAppService<TargetDefinitionDto, Guid>
    { 
    
    }
}

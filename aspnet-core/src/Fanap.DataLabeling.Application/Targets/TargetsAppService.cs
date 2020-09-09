using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Targets
{
    [AbpAuthorize]
    public class TargetsAppService : AsyncCrudAppService<TargetDefinition, TargetDefinitionDto, Guid>, ITargetsAppService
    {
        public TargetsAppService(IRepository<TargetDefinition, Guid> repository) : base(repository)
        {
        }

        public override Task<TargetDefinitionDto> CreateAsync(TargetDefinitionDto input)
        {
            input.OwnerId = AbpSession.UserId.Value;
            return base.CreateAsync(input);
        }

    }
}

using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fanap.DataLabeling.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Targets
{
    [AbpAuthorize]
    public class TargetsAppService : AsyncCrudAppService<TargetDefinition, TargetDefinitionDto, Guid, TargetDefinitionGetAllInput>, ITargetsAppService
    {
        public TargetsAppService(IRepository<TargetDefinition, Guid> repository) : base(repository)
        {
        }
        protected override IQueryable<TargetDefinition> CreateFilteredQuery(TargetDefinitionGetAllInput input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(input.DataSetId != null, ff => ff.DataSetId == input.DataSetId.Value)
                .WhereIf(input.OwnerId != null, ff => ff.OwnerId == input.OwnerId.Value);
        }

        public override Task<TargetDefinitionDto> CreateAsync(TargetDefinitionDto input)
        {
            input.OwnerId = AbpSession.UserId.Value;
            return base.CreateAsync(input);
        }

    }

}

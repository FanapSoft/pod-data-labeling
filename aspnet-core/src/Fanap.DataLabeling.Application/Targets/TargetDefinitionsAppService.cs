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
    public class TargetDefinitionsAppService : AsyncCrudAppService<TargetDefinition, TargetDefinitionDto, Guid, TargetDefinitionGetAllInput>, ITargetDefinitionsAppService
    {
        public TargetDefinitionsAppService(IRepository<TargetDefinition, Guid> repository) : base(repository)
        {
        }

        protected override IQueryable<TargetDefinition> CreateFilteredQuery(TargetDefinitionGetAllInput input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(input.DataSetId != null, ff => ff.DataSetId == input.DataSetId.Value);
        }
    }
}

using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Targets
{
    [AbpAuthorize]
    public class TargetsAppService : AsyncCrudAppService<UserTarget, UserTargetDto, Guid, TargetGetAllInput>
    {
        public TargetsAppService(IRepository<UserTarget, Guid> repository) : base(repository)
        {
        }

        protected override IQueryable<UserTarget> CreateFilteredQuery(TargetGetAllInput input)
        {
            return base.CreateFilteredQuery(input)
                .Where(ff => ff.OwnerId == input.OwnerId);
        }
        public async override Task<UserTargetDto> CreateAsync(UserTargetDto input)
        {
            var result = await Repository.InsertAsync(new UserTarget
            {
                TargetDefinitionId = input.TargetDefinitionId,
                OwnerId = AbpSession.UserId.Value

            });
            return MapToEntityDto(result);
        }
    }
}

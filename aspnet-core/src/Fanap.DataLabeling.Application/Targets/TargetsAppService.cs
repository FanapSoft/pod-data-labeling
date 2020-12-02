using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Fanap.DataLabeling.Datasets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Targets
{
    public class TargetStatusOutput
    {
        public bool TargetEnded { get; set; }
        public bool NoTarget { get; set; }

    }
    public class TargetStatusInput
    {
        [Required]
        public Guid DataSetId { get; set; }
    }
    [AbpAuthorize]
    public class TargetsAppService : AsyncCrudAppService<UserTarget, UserTargetDto, Guid, TargetGetAllInput>
    {
        private readonly IRepository<AnswerLog, Guid> answerLogRepo;

        public TargetsAppService(IRepository<AnswerLog, Guid> answerLogRepo, IRepository<UserTarget, Guid> repository) : base(repository)
        {
            this.answerLogRepo = answerLogRepo;
        }

        protected override IQueryable<UserTarget> CreateFilteredQuery(TargetGetAllInput input)
        {
            if (input.DataSetId == null && input.OwnerId == null)
                throw new UserFriendlyException("OwnerId or DataSetId is required.");
            return base.CreateFilteredQuery(input)
                .WhereIf(input.DataSetId != null, ff => ff.TargetDefinition.DataSetId == input.DataSetId.Value)
                .WhereIf(input.OwnerId != null, ff => ff.OwnerId == input.OwnerId.Value);
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

        public async Task<TargetStatusOutput> GetCurrentTargetStatus(TargetStatusInput input)
        {
            var userId = AbpSession.UserId.Value;
            var userSpecificTarget = await Repository.GetAllIncluding(ff => ff.TargetDefinition).OrderBy(ff => ff.CreationTime).LastOrDefaultAsync(ff => ff.TargetDefinition.DataSetId == input.DataSetId && ff.OwnerId == userId);
            if (userSpecificTarget == null)
                return new TargetStatusOutput
                {
                    NoTarget = true
                };

            var totalUsersAnswer = answerLogRepo.Count(ff => ff.CreatorUserId == userId && ff.DataSetId == input.DataSetId);
            if (totalUsersAnswer >= userSpecificTarget.TargetDefinition.AnswerCount)
                return new TargetStatusOutput
                {
                    TargetEnded = true
                };

            return new TargetStatusOutput
            {
                NoTarget = false,
                TargetEnded = false
            };
        }
    }
}

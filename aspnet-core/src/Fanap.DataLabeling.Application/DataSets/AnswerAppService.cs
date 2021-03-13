using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Fanap.DataLabeling.Authorization;
using Fanap.DataLabeling.Datasets;
using Fanap.DataLabeling.Targets;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{

    public class SubmitBatchAnswerInput
    {
        public List<SubmitAnswerInput> Answers { get; set; }
    }
    public class SubmitBatchAnswerOutput
    {
        public List<SubmitAnswerOutput> Answers { get; set; }
    }
    public class GetAllAnswerLogsInput : PagedAndSortedResultRequestDto
    {
        public bool IncludeQuestion { get; set; }
        public Guid? DataSetId { get; set; }
        public long? UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
    [AbpAuthorize]

    public class AnswersAppService : AsyncCrudAppService<AnswerLog, AnswerLogDto, Guid, GetAllAnswerLogsInput>, IAnswersAppService
    {
        private readonly IRepository<UserTarget, Guid> targetRepo;
        private readonly IRepository<Dataset, Guid> datasetRepo;
        private readonly IRepository<AnswerLog, Guid> answerLogRepo;
        private readonly IQuestionsAppService questionAppService;

        public AnswersAppService(
            IRepository<UserTarget, Guid> targetRepo,
            IRepository<AnswerLog, Guid> repository,
            IRepository<Dataset, Guid> datasetRepo,
            IRepository<DatasetItem, Guid> datasetItemRepo,
            IRepository<AnswerLog, Guid> answerLogRepo,
            IQuestionsAppService questionAppService) : base(repository)
        {
            this.CreatePermissionName = PermissionNames.Pages_Roles;
            this.DeletePermissionName = PermissionNames.Pages_Roles;
            this.targetRepo = targetRepo;
            this.datasetRepo = datasetRepo;
            this.answerLogRepo = answerLogRepo;
            this.questionAppService = questionAppService;
        }

        protected override IQueryable<AnswerLog> CreateFilteredQuery(GetAllAnswerLogsInput input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(input.DataSetId != null, ff => ff.DataSetId == input.DataSetId)
                .WhereIf(input.UserId != null, ff => ff.CreatorUserId == input.UserId)
                .WhereIf(input.From != null, ff => ff.CreationTime >= input.From)
                .WhereIf(input.To != null, ff => ff.CreationTime <= input.To);
        }

        public override async Task<PagedResultDto<AnswerLogDto>> GetAllAsync(GetAllAnswerLogsInput input)
        {
            var result = await base.GetAllAsync(input);
            if (input.IncludeQuestion)
                await SetAnswerLabel(result);
            return result;
        }
        public override Task<AnswerLogDto> UpdateAsync(AnswerLogDto input)
        {
            var dataset = datasetRepo.Get(input.DataSetId);
            if (dataset.LabelingStatus == LabelingStatus.Finished)
                throw new UserFriendlyException("You cannot edit the answer at this stage.");
            return base.UpdateAsync(input);
        }
        private async Task SetAnswerLabel(PagedResultDto<AnswerLogDto> result)
        {
            foreach (var item in result.Items)
            {             
                item.Title = await questionAppService.GetItemLabel(item.DataSetItemId);
            }
        }
        private async Task SetQuestion(PagedResultDto<AnswerLogDto> result)
        {
            var serializerSetting = new JsonSerializerSettings();
            serializerSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializerSetting.NullValueHandling = NullValueHandling.Ignore;
            foreach (var item in result.Items)
            {
                var question = await questionAppService.GetQuestion(new GetQuestionInput { DataSetId = item.DataSetId, DataSetItemId = item.DataSetItemId });
                item.QuestionObject = JsonConvert.SerializeObject(question, serializerSetting);
            }
        }

        public async Task<SubmitBatchAnswerOutput> SubmitBatchAnswer(SubmitBatchAnswerInput input)
        {
            if (input.Answers == null || !input.Answers.Any())
                throw new UserFriendlyException("You need to provide answers");
            var result = new List<SubmitAnswerOutput>();
            foreach (var item in input.Answers)
            {
                result.Add(await SubmitAnswer(item));
            }
            return new SubmitBatchAnswerOutput
            {
                Answers = result
            };
        }
        public async Task<SubmitAnswerOutput> SubmitAnswer(SubmitAnswerInput input)
        {
            var userId = AbpSession.UserId.Value;
            var foundDataSet = datasetRepo.GetAll().SingleOrDefault(ff => ff.Id == input.DataSetId);
            if (foundDataSet == null)
                throw new UserFriendlyException("Dataset not found");
            if (!foundDataSet.IsActive)
                throw new UserFriendlyException("Dataset is not active");
            if (foundDataSet.LabelingStatus != LabelingStatus.Started)
                throw new UserFriendlyException("Dataset is not in labeling mode.");
            if (input.Ignored && input.IgnoreReason.IsNullOrEmpty())
                throw new UserFriendlyException("Ignore reason is required.");

            var userSpecificTarget = await targetRepo.GetAllIncluding(ff => ff.TargetDefinition).OrderBy(ff => ff.CreationTime).LastOrDefaultAsync(ff => ff.TargetDefinition.DataSetId == input.DataSetId && ff.OwnerId == userId);
            if (userSpecificTarget == null)
                throw new UserFriendlyException("User does not have an active target.");

            var totalUsersAnswer = answerLogRepo.Count(ff => ff.CreatorUserId == userId && ff.DataSetId == input.DataSetId);
            if (totalUsersAnswer >= userSpecificTarget.TargetDefinition.AnswerCount)
                return new SubmitAnswerOutput
                {
                    TargetEnded = true
                };

            var log = new AnswerLog()
            {
                Answer = input.AnswerIndex,
                DurationToAnswerInSeconds = input.DurationToAnswerInSeconds,
                DataSetId = input.DataSetId,
                DataSetItemId = input.DataSetItemId,
                Ignored = input.Ignored,
                IgnoreReason = input.IgnoreReason,
                //QuestionObject = JsonConvert.SerializeObject(question, Formatting.Indented, serializerSetting),
            };

            var id = await answerLogRepo.InsertAndGetIdAsync(log);
            return new SubmitAnswerOutput { Id = id };
        }
        public async Task<AnswerStatisticsOutput> Stats(AnswerStatisticsInput input)
        {
            var query = await Repository
                .GetAll()
                .WhereIf(input.UserId != null, ff => ff.CreatorUserId == input.UserId.Value)
                .WhereIf(input.DataSetId != null, ff => ff.DataSetId == input.DataSetId.Value)
                .CountAsync();

            return new AnswerStatisticsOutput { TotalCount = query };
        }
    }
}

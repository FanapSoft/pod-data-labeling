using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Fanap.DataLabeling.Authorization;
using Fanap.DataLabeling.Datasets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Guid? DataSetId { get; set; }
        public long? UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
    [AbpAuthorize]

    public class AnswersAppService : AsyncCrudAppService<AnswerLog, AnswerLogDto, Guid, GetAllAnswerLogsInput>, IAnswersAppService
    {
        private readonly IRepository<Dataset, Guid> datasetRepo;
        private readonly IRepository<AnswerLog, Guid> answerLogRepo;
        private readonly IQuestionsAppService questionAppService;

        public AnswersAppService(
            IRepository<AnswerLog, Guid> repository,
            IRepository<Dataset, Guid> datasetRepo,
            IRepository<DatasetItem, Guid> datasetItemRepo,
            IRepository<AnswerLog, Guid> answerLogRepo,
            IQuestionsAppService questionAppService) : base(repository)
        {
            this.CreatePermissionName = PermissionNames.Pages_Roles;
            this.UpdatePermissionName = PermissionNames.Pages_Roles;
            this.DeletePermissionName = PermissionNames.Pages_Roles;
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
            var foundDataSet = datasetRepo.GetAll().SingleOrDefault(ff => ff.Id == input.DataSetId);
            if (foundDataSet == null)
                throw new UserFriendlyException("Dataset not found");
            if (!foundDataSet.IsActive)
                throw new UserFriendlyException("Dataset is not active");
            if (foundDataSet.LabelingStatus != LabelingStatus.Started)
                throw new UserFriendlyException("Dataset is not in labeling mode.");
            if (input.Ignored && input.IgnoreReason.IsNullOrEmpty())
                throw new UserFriendlyException("Ignore reason is required.");


            var question = questionAppService.GetQuestion(new GetQuestionInput { DataSetId = input.DataSetId, DataSetItemId = input.DataSetItemId });

            var log = new AnswerLog()
            {
                Answer = input.AnswerIndex,
                DataSetId = input.DataSetId,
                DataSetItemId = input.DataSetItemId,
                Ignored = input.Ignored,
                IgnoreReason = input.IgnoreReason,
                QuestionObject = JsonConvert.SerializeObject(question),
            };

            var id = await answerLogRepo.InsertAndGetIdAsync(log);
            return new SubmitAnswerOutput { Id = id };
        }
    }
}

using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Fanap.DataLabeling.Datasets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public class AnswersAppService : ApplicationService, IAnswersAppService
    {
        private readonly IRepository<Dataset, Guid> datasetRepo;
        private readonly IRepository<AnswerLog, Guid> answerLogRepo;
        private readonly IQuestionsAppService questionAppService;

        public AnswersAppService(
            IRepository<Dataset, Guid> datasetRepo,
            IRepository<DatasetItem, Guid> datasetItemRepo,
            IRepository<AnswerLog, Guid> answerLogRepo,
            IQuestionsAppService questionAppService)
        {
            this.datasetRepo = datasetRepo;
            this.answerLogRepo = answerLogRepo;
            this.questionAppService = questionAppService;
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
            if(input.Ignored && input.IgnoreReason.IsNullOrEmpty())
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

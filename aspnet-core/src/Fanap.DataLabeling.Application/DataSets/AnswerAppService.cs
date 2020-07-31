using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Datasets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    [AbpAuthorize]
    public class AnswersAppService : ApplicationService, IAnswersAppService
    {
        private readonly IRepository<AnswerLog, Guid> answerLogRepo;
        private readonly IQuestionsAppService questionAppService;

        public AnswersAppService(IRepository<AnswerLog, Guid> answerLogRepo, IQuestionsAppService questionAppService)
        {
            this.answerLogRepo = answerLogRepo;
            this.questionAppService = questionAppService;
        }
        public async Task<SubmitAnswerOutput> SubmitAnswer(SubmitAnswerInput input)
        {
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

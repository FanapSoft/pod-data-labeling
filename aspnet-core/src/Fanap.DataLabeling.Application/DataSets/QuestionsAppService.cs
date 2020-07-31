using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Datasets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    [AbpAuthorize]
    public class QuestionsAppService : ApplicationService, IQuestionsAppService
    {
        private readonly IRepository<Dataset, Guid> dataSetRepo;
        private readonly IRepository<DatasetItem, Guid> dataSetItemRepo;

        public QuestionsAppService(IRepository<Dataset, Guid> dataSetRepo, IRepository<DatasetItem, Guid> dataSetItemRepo)
        {
            this.dataSetRepo = dataSetRepo;
            this.dataSetItemRepo = dataSetItemRepo;
        }
        public async Task<QuestionDto> GetQuestion(GetQuestionInput input)
        {
            var dataSet = await dataSetRepo
                .GetAllIncluding(ff => ff.AnswerOptions)
                .SingleOrDefaultAsync(ff => ff.Id == input.DataSetId);
            var dataSetItem = await dataSetItemRepo.GetAsync(input.DataSetItemId);

            var question = new QuestionDto()
            {
                QuestionType = dataSet.QuestionType,
                AnswerType = dataSet.AnswerType,
                Options = ObjectMapper.Map<List<AnswerOptionDto>>(dataSet.AnswerOptions),
            };
            SetQuestionInfo(question, dataSet, dataSetItem);
            return question;
        }

        private void SetQuestionInfo(QuestionDto question, Dataset dataSet, DatasetItem dataSetItem)
        {
            if (question.QuestionType == QuestionType.Text)
            {
                question.Title = dataSet.QuestionTemplate.Replace("{{Label.Title}}", dataSetItem.Label.Name);
            }
            else if (question.QuestionType == QuestionType.Video || question.QuestionType == QuestionType.Voice)
            {
                question.Src = dataSet.QuestionSrc;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(question.QuestionType));
            }
        }
    }
}

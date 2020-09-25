﻿using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Fanap.DataLabeling.Datasets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public class GetQuestionsInput
    {

        public int Count { get; set; }
        public Guid DataSetId { get; set; }

    }
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
        public async Task<List<QuestionDto>> GetQuestions(GetQuestionsInput input)
        {
            var dataSet = await dataSetRepo
                .GetAllIncluding(ff => ff.AnswerOptions)
                .SingleOrDefaultAsync(ff => ff.Id == input.DataSetId);

            if (dataSet == null)
                throw new UserFriendlyException($"DataSet not found with id {input.DataSetId}");
            if (dataSet.AnswerOptions == null || !dataSet.AnswerOptions.Any())
                throw new UserFriendlyException($"DataSet doest not have its answer options configured");

            // random dataset items
            var dataSetItems = await dataSetItemRepo.GetAll().OrderBy(ff => Guid.NewGuid()).Take(input.Count).ToListAsync();

            var res = dataSetItems.Select(ff =>
            {
                return GetQuestion(new GetQuestionInput { DataSetId = input.DataSetId, DataSetItemId = ff.Id }).Result;
            });
            return res.ToList();
        }
        public async Task<QuestionDto> GetQuestion(GetQuestionInput input)
        {
            var dataSet = await dataSetRepo
                .GetAllIncluding(ff => ff.AnswerOptions)
                .SingleOrDefaultAsync(ff => ff.Id == input.DataSetId);

            if (dataSet == null)
                throw new UserFriendlyException($"DataSet not found with id {input.DataSetId}");
            if (dataSet.AnswerOptions == null || !dataSet.AnswerOptions.Any())
                throw new UserFriendlyException($"DataSet doest not have its answer options configured");

            var dataSetItem = await dataSetItemRepo.GetAll().SingleOrDefaultAsync(ff => ff.Id == input.DataSetItemId);
            if (dataSetItem == null)
                throw new UserFriendlyException($"DataSetItem not found with id {input.DataSetItemId}");

            var question = new QuestionDto()
            {
                DatasetItemId = dataSetItem.Id,
                QuestionType = dataSet.QuestionType,
                AnswerType = dataSet.AnswerType,
                Options = ObjectMapper.Map<List<AnswerOptionDto>>(dataSet.AnswerOptions),
            };
            SetQuestionInfo(question, dataSet, dataSetItem);
            return question;
        }

        private void SetQuestionInfo(QuestionDto question, Dataset dataSet, DatasetItem dataSetItem)
        {
            question.QuestionSubjectFileSrc = dataSetItem.Id.ToString();
            if (question.QuestionType == QuestionType.Text)
            {
                if (dataSet.QuestionTemplate.IsNullOrEmpty())
                    throw new UserFriendlyException("Question template of dataset is empty");
                //if (!dataSet.QuestionTemplate.Contains("{{Label.Title}}"))
                //    throw new UserFriendlyException("Question template does not have {{Label.Title}} placeholder (case sensitive).");

                // TODO: CHANGE TO LABEL NAME
                question.Title = dataSet.QuestionTemplate.Replace("{{Label.Title}}", dataSetItem.Name);
            }
            if (question.QuestionType == QuestionType.Image || question.QuestionType == QuestionType.Video || question.QuestionType == QuestionType.Voice)
            {
                question.QuestionFileSrc = dataSet.QuestionSrc;
            }
        }
    }
}

using Abp.Domain.Repositories;
using Abp.UI;
using Fanap.DataLabeling.Datasets;
using Fanap.DataLabeling.Targets;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Credit
{
    public class CreditAppService : DataLabelingAppServiceBase
    {
        private readonly IRepository<TargetDefinition, Guid> targetRepo;
        private readonly IRepository<AnswerLog, Guid> answerRepo;
        private readonly IRepository<DatasetItem, Guid> datasetItemRepo;
        private readonly IRepository<Dataset, Guid> datasetRepo;

        public CreditAppService(IRepository<TargetDefinition, Guid> targetRepo, IRepository<AnswerLog, Guid> answerRepo, IRepository<DatasetItem, Guid> datasetItemRepo, IRepository<Dataset, Guid> datasetRepo)
        {
            this.targetRepo = targetRepo;
            this.answerRepo = answerRepo;
            this.datasetItemRepo = datasetItemRepo;
            this.datasetRepo = datasetRepo;
        }

        public async Task<GetCreditOutput> GetCredit(GetCreditInput input)
        {
            var dataset = datasetRepo.Get(input.DataSetId);
            var allGoldenAnswers = answerRepo
                .GetAll()
                .Where(ff => ff.DataSetId == input.DataSetId && ff.CreatorUserId == input.UserId && ff.DataSetItem.IsGoldenData && !ff.Ignored).Select(ff => new { ff.Id, ff.Answer });
            var goldenAnswersCount = await allGoldenAnswers.CountAsync();
            var targetTotalCount = dataset.AnswerBudgetCountPerUser == 0 ? 1000: dataset.AnswerBudgetCountPerUser;
            var targetGoldenCount = GetTargetGoldenCount(targetTotalCount, dataset);
            var correctGoldenAsnwersToCredit = new List<dynamic>();
            var incorrectCorrectGoldenAsnwersToCredit = new List<dynamic>();

            var N = targetTotalCount;
            var G = targetGoldenCount;
            var UMax = GetTargetUMax(targetTotalCount, dataset);
            var UMin = GetTargetUMin(targetTotalCount, dataset);
            var T = dataset.T;
            foreach (var item in allGoldenAnswers.ToList())
            {
                if (item.Answer != dataset.CorrectGoldenAnswerIndex)
                    incorrectCorrectGoldenAsnwersToCredit.Add(item);

                correctGoldenAsnwersToCredit.Add(item);
            }
            var correctGoldenAnswersCount = correctGoldenAsnwersToCredit.Count;
            var middle = Convert.ToDouble(UMax - UMin);
            if (correctGoldenAnswersCount == 0)
                return new GetCreditOutput
                {
                    Credit = dataset.UMin
                };
            if (correctGoldenAnswersCount > targetTotalCount)
                return new GetCreditOutput
                {
                    Credit = dataset.UMax
                };
            var bonusesFraction = G - correctGoldenAnswersCount;
            var result = middle * (Math.Pow(Convert.ToDouble(T), Convert.ToDouble(bonusesFraction))) + Convert.ToDouble(UMin);
            return new GetCreditOutput { Credit = Convert.ToDecimal(Math.Round(result / 1000, 0) * 1000), All = G, Correct = correctGoldenAnswersCount };
        }

        private int GetTargetUMin(int targetTotalCount, Dataset dataset)
        {
            var totalItemcount = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id);
            var totalGoldenCount = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id && ff.IsGoldenData);
            var fraction = Convert.ToDouble(targetTotalCount) / Convert.ToDouble(totalItemcount);
            return Convert.ToInt32(Convert.ToDouble(dataset.UMin) * fraction);
        }

        private int GetTargetUMax(int targetTotalCount, Dataset dataset)
        {
            var totalItemcount = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id);
            var totalGoldenCount = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id && ff.IsGoldenData);
            var fraction = Convert.ToDouble(targetTotalCount) / Convert.ToDouble(totalItemcount);
            return Convert.ToInt32(Convert.ToDouble(dataset.UMax) * fraction);
        }

        private int GetTargetGoldenCount(int targetTotalCount, Dataset dataset)
        {
            var totalItemcount = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id);
            var totalGoldenCount = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id && ff.IsGoldenData);
            var fraction = Convert.ToDouble(targetTotalCount) / Convert.ToDouble(totalItemcount);
            return Convert.ToInt32(totalGoldenCount * fraction);
        }

        public async Task<GetCreditOutput> CollectCredit(GetCreditInput input)
        {
            throw new NotImplementedException("should insert proper transaction based on credit");
        }
    }
}

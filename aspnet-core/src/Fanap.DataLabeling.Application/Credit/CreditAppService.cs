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
            var userSpecificTarget = await targetRepo.GetAll().OrderBy(ff => ff.CreationTime).LastOrDefaultAsync(ff => ff.DataSetId == input.DataSetId && ff.OwnerId == input.UserId);
            if (userSpecificTarget == null)
                throw new UserFriendlyException("There is no user specific target assigned to the current user in this dataset.");

            var answerBudgetPerUser = userSpecificTarget.AnswerCount;

            var targetGoldenCount = GetTargetGoldenCount(answerBudgetPerUser, dataset);
            var correctGoldenAsnwersToCredit = new List<dynamic>();
            var incorrectCorrectGoldenAsnwersToCredit = new List<dynamic>();
            var G = targetGoldenCount;
            var UMax = GetTargetMoniteryLimit(answerBudgetPerUser, dataset, dataset.UMax);
            var UMin = GetTargetMoniteryLimit(answerBudgetPerUser, dataset, dataset.UMin);
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
            if (correctGoldenAnswersCount > answerBudgetPerUser)
                return new GetCreditOutput
                {
                    Credit = dataset.UMax
                };
            var bonusesFraction = G - correctGoldenAnswersCount;
            var result = middle * (Math.Pow(Convert.ToDouble(T), Convert.ToDouble(bonusesFraction))) + Convert.ToDouble(UMin);
            return new GetCreditOutput
            {
                UMin  = UMin,
                Umax = UMax,
                Credit = Convert.ToDecimal(Math.Round(result / 10, 0) * 10),
                G = G,
                GoldCounts = targetGoldenCount,
                Correct = correctGoldenAnswersCount
            };
        }

        private int GetTargetMoniteryLimit(int answerBudgetPerUser, Dataset dataset, decimal value)
        {
            var totalItem = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id);
            var totalGolden = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id && ff.IsGoldenData);
            var generalFraction = Convert.ToDouble(totalGolden) / Convert.ToDouble(totalItem);

            var totalReplicatedGolden = dataset.AnswerReplicationCount * totalGolden;

            var targetGolden = GetTargetGoldenCount(answerBudgetPerUser, dataset);
            var result = (Convert.ToDouble(targetGolden) * Convert.ToDouble(value)) / Convert.ToDouble(totalReplicatedGolden);

            return Convert.ToInt32(result);
        }

        private int GetTargetGoldenCount(int answerBudgetPerUser, Dataset dataset)
        {
            var totalItemcount = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id);
            var totalGoldenCount = datasetItemRepo.Count(ff => ff.DatasetID == dataset.Id && ff.IsGoldenData);
            var fraction = Convert.ToDouble(totalGoldenCount) / Convert.ToDouble(totalItemcount);
            return Convert.ToInt32(answerBudgetPerUser * fraction);
        }

        public async Task<GetCreditOutput> CollectCredit(GetCreditInput input)
        {
            throw new NotImplementedException("should insert proper transaction based on credit");
        }
    }
}

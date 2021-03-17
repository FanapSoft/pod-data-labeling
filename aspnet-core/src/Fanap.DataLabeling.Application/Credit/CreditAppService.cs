using Abp.Domain.Repositories;
using Abp.UI;
using Fanap.DataLabeling.Accounting;
using Fanap.DataLabeling.Authorization.Users;
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
        private readonly IRepository<User, long> userRepo;
        private readonly IRepository<Transaction, Guid> transactionRepo;
        private readonly IRepository<UserTarget, Guid> targetRepo;
        private readonly IRepository<AnswerLog, Guid> answerRepo;
        private readonly IRepository<DatasetItem, Guid> datasetItemRepo;
        private readonly IRepository<Dataset, Guid> datasetRepo;

        public CreditAppService(IRepository<User, long> userRepo, IRepository<Transaction, Guid> transactionRepo, IRepository<UserTarget, Guid> targetRepo, IRepository<AnswerLog, Guid> answerRepo, IRepository<DatasetItem, Guid> datasetItemRepo, IRepository<Dataset, Guid> datasetRepo)
        {
            this.userRepo = userRepo;
            this.transactionRepo = transactionRepo;
            this.targetRepo = targetRepo;
            this.answerRepo = answerRepo;
            this.datasetItemRepo = datasetItemRepo;
            this.datasetRepo = datasetRepo;
        }

        public async Task<GetCreditOutput> GetCredit(GetCreditInput input, bool checkTarget = false)
        {
            var dataset = datasetRepo.Get(input.DataSetId);
            var allAnswers = answerRepo
                .GetAll()
                .Where(ff => !ff.CreditCalculated && !ff.Ignored && ff.DataSetId == input.DataSetId && ff.CreatorUserId == input.UserId);

            var allGoldenAnswers = allAnswers.Where(ff => ff.DataSetItem.IsGoldenData).Select(ff => new { ff.Id, ff.Answer });

            var userSpecificTarget = await targetRepo.GetAllIncluding(ff => ff.TargetDefinition.DataSet).OrderBy(ff => ff.CreationTime).LastOrDefaultAsync(ff => ff.TargetDefinition.DataSetId == input.DataSetId && ff.OwnerId == input.UserId);
            if (userSpecificTarget == null)
                throw new UserFriendlyException("There is no user specific target assigned to the current user in this dataset.");

            var answerBudgetPerUser = userSpecificTarget.TargetDefinition.AnswerCount;

            if (checkTarget && (allAnswers.Count() < answerBudgetPerUser))
            {
                throw new UserFriendlyException("You haven't reached the target yet.");
            }

            var targetGoldenCount = userSpecificTarget.TargetDefinition.GoldenCount;
            var correctGoldenAsnwersToCredit = new List<dynamic>();
            var incorrectGoldenAsnwersToCredit = new List<dynamic>();
            var G = targetGoldenCount;
            var UMax = userSpecificTarget.TargetDefinition.UMax;
            var UMin = userSpecificTarget.TargetDefinition.UMin;
            var bonusTrue = userSpecificTarget.TargetDefinition.BonusTrue;
            var bonusFalse = userSpecificTarget.TargetDefinition.BonusFalse;
            var T = userSpecificTarget.TargetDefinition.T;
            foreach (var item in allGoldenAnswers.ToList())
            {
                if (item.Answer != dataset.CorrectGoldenAnswerIndex)
                    incorrectGoldenAsnwersToCredit.Add(item);
                else
                    correctGoldenAsnwersToCredit.Add(item);
            }
            var correctGoldenAnswersCount = correctGoldenAsnwersToCredit.Count;
            var middle = Convert.ToDouble(UMax - UMin) * Math.Pow(T, Convert.ToDouble(G));
            if (correctGoldenAnswersCount == 0)
                return new GetCreditOutput
                {
                    Credit = UMin
                };
            if (correctGoldenAnswersCount > answerBudgetPerUser)
                return new GetCreditOutput
                {
                    Credit = UMax
                };
            var result = middle * Math.Pow(bonusFalse, incorrectGoldenAsnwersToCredit.Count) * Math.Pow(bonusTrue, correctGoldenAnswersCount) + Convert.ToDouble(UMin);

            var dto = ObjectMapper.Map<TargetDefinitionDto>(userSpecificTarget.TargetDefinition);
            dto.DataSet = null;
            return new GetCreditOutput
            {
                Correct = correctGoldenAnswersCount,
                Incorrect = incorrectGoldenAsnwersToCredit.Count,
                Middle = middle,
                Target = dto,
                Credit = result,
            };
        }

        public async Task<TransactionDto> CollectCredit(GetCreditInput input)
        {
            var dataset = await datasetRepo.GetAsync(input.DataSetId);

            var creditResult = await GetCredit(input, true);

            var transaction = new Transaction
            {
                CreditAmount = creditResult.Credit,
                OwnerId = input.UserId,
                Reason = TransactionReason.CollectCredit,
                ReferenceDataSetId = input.DataSetId
            };

            transaction = transactionRepo.Insert(transaction);

            var allGoldenAnswers = await answerRepo.GetAll()
                .Where(ff => !ff.CreditCalculated && ff.DataSetId == input.DataSetId && ff.CreatorUserId == input.UserId && ff.DataSetItem.IsGoldenData && !ff.Ignored)
                .Select(ff => new { ff.Id })
                .ToListAsync();

            foreach (var item in allGoldenAnswers)
            {
                answerRepo.Update(item.Id, ff => ff.CreditCalculated = true);
            }

            var userSpecificTarget = targetRepo.GetAllIncluding(ff => ff.TargetDefinition.DataSet)
                                     .Where(ff => ff.TargetDefinition.DataSetId == input.DataSetId && ff.OwnerId == input.UserId).ToList();

            userSpecificTarget.ForEach(_ => _.IsDeleted = true);

            CurrentUnitOfWork.SaveChanges();

            return ObjectMapper.Map<TransactionDto>(transaction);
        }
    }
}

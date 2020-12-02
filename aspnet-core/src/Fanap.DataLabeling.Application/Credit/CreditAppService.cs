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

        public async Task<GetCreditOutput> GetCredit(GetCreditInput input)
        {
            var dataset = datasetRepo.Get(input.DataSetId);
            var allGoldenAnswers = answerRepo
                .GetAll()
                .Where(ff => ff.DataSetId == input.DataSetId && ff.CreatorUserId == input.UserId && ff.DataSetItem.IsGoldenData && !ff.Ignored).Select(ff => new { ff.Id, ff.Answer });
            var goldenAnswersCount = await allGoldenAnswers.CountAsync();
            var userSpecificTarget = await targetRepo.GetAllIncluding(ff => ff.TargetDefinition.DataSet).OrderBy(ff => ff.CreationTime).LastOrDefaultAsync(ff => ff.TargetDefinition.DataSetId == input.DataSetId && ff.OwnerId == input.UserId);
            if (userSpecificTarget == null)
                throw new UserFriendlyException("There is no user specific target assigned to the current user in this dataset.");

            var answerBudgetPerUser = userSpecificTarget.TargetDefinition.AnswerCount;

            var targetGoldenCount = userSpecificTarget.TargetDefinition.GoldenCount;
            var correctGoldenAsnwersToCredit = new List<dynamic>();
            var incorrectCorrectGoldenAsnwersToCredit = new List<dynamic>();
            var G = targetGoldenCount;
            var UMax = userSpecificTarget.TargetDefinition.UMax;
            var UMin = userSpecificTarget.TargetDefinition.UMin;
            var T = userSpecificTarget.TargetDefinition.T;
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
                    Credit = UMin
                };
            if (correctGoldenAnswersCount > answerBudgetPerUser)
                return new GetCreditOutput
                {
                    Credit = UMax
                };
            var bonusesFraction = G - correctGoldenAnswersCount;
            var result = middle * (Math.Pow(Convert.ToDouble(T), Convert.ToDouble(bonusesFraction))) + Convert.ToDouble(UMin);
            var dto = ObjectMapper.Map<TargetDefinitionDto>(userSpecificTarget.TargetDefinition);
            dto.DataSet = null;
            return new GetCreditOutput
            {
                Target = dto,
                Credit = result,
            };
        }

        public async Task<TransactionDto> CollectCredit(GetCreditInput input)
        {
            var dataset = await datasetRepo.GetAsync(input.DataSetId);

            if (!dataset.IsActive || dataset.LabelingStatus != LabelingStatus.Finished)
                throw new UserFriendlyException("برای دریافت مبلغ اعتبار، عملیات برچسب زنی دیتاست باید تمام شده باشد.");
            var creditResult = await GetCredit(input);
            var alreadyCollected = transactionRepo.GetAll().Any(ff => ff.OwnerId == input.UserId && ff.ReferenceDataSetId == input.DataSetId && ff.Reason == TransactionReason.CollectCredit);
            if (alreadyCollected)
                throw new UserFriendlyException("مبلغ اعتبار این دیتاست قبلا جمع‌آوری شده است.");

            var transaction = new Transaction
            {
                CreditAmount = Math.Round(creditResult.Credit / 1000) * 1000,
                OwnerId = input.UserId,
                Reason = TransactionReason.CollectCredit,
                ReferenceDataSetId = input.DataSetId
            };
            transaction = transactionRepo.Insert(transaction);

            return ObjectMapper.Map<TransactionDto>(transaction);
        }
    }
}

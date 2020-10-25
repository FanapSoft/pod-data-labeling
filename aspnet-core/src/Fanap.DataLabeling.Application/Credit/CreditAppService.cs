using Abp.Domain.Repositories;
using Fanap.DataLabeling.Datasets;
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
        private readonly IRepository<AnswerLog, Guid> answerRepo;
        private readonly IRepository<DatasetItem, Guid> datasetItemRepo;
        private readonly IRepository<Dataset, Guid> datasetRepo;

        public CreditAppService(IRepository<AnswerLog, Guid> answerRepo, IRepository<DatasetItem, Guid> datasetItemRepo, IRepository<Dataset, Guid> datasetRepo)
        {
            this.answerRepo = answerRepo;
            this.datasetItemRepo = datasetItemRepo;
            this.datasetRepo = datasetRepo;
        }

        public async Task<GetCreditOutput> GetCredit(GetCreditInput input)
        {
            var goldenAnswers = answerRepo
                .GetAll()
                .Where(ff => ff.DataSetId == input.DataSetId && ff.CreatorUserId == input.UserId && ff.DataSetItem.IsGoldenData && !ff.Ignored);
            var goldenAnswersCount = await goldenAnswers.CountAsync();
            var dataset = datasetRepo.Get(input.DataSetId);
            var middle = (dataset.UMax - dataset.UMin) * (Convert.ToInt64(dataset.T) ^ goldenAnswersCount);
            var incorrectAnswerCount = goldenAnswers.Count(ff => ff.Answer != dataset.CorrectGoldenAnswerIndex);
            if (incorrectAnswerCount > 0)
                return new GetCreditOutput
                {
                    Credit = dataset.UMin
                };
            var correctCount = goldenAnswers.Where(ff => ff.Answer == dataset.CorrectGoldenAnswerIndex);
            var bonuses = Convert.ToDecimal(correctCount) * (1 / dataset.T);

            var result = middle * bonuses + dataset.UMin;
            return new GetCreditOutput { Credit = result };
        }

        public async Task<GetCreditOutput> CollectCredit(GetCreditInput input)
        {
            throw new NotImplementedException("should insert proper transaction based on credit");
        }
    }
}

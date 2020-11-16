using Abp.Authorization;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Fanap.DataLabeling.Authorization.Users;

namespace Fanap.DataLabeling.Reports
{
    public class AnswersCountsInput
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? DataSetId { get; set; }
    }
    public class ScoreboardInput
    {
        public DateTime? From { get; set; }
        public Guid? DataSetId { get; set; }
    }
    public class AnswersCountsOutput
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public long UserId { get; set; }
        public long Count { get; set; }
    }
    [AbpAuthorize]
    public class ReportsAppService : DataLabelingAppServiceBase
    {
        private readonly IRepository<AnswerLog, Guid> answerLogRepo;
        private readonly IRepository<User, long> usersRepo;

        public ReportsAppService(IRepository<AnswerLog, Guid> answerLogRepo, IRepository<User, long> usersRepo)
        {
            this.answerLogRepo = answerLogRepo;
            this.usersRepo = usersRepo;
        }

        public async Task<IEnumerable<AnswersCountsOutput>> AnswersCounts(AnswersCountsInput input)
        {
            if (input.From == null)
                input.From = DateTime.UtcNow.Date.AddDays(-7);
            if (input.To == null)
                input.To = DateTime.UtcNow.AddDays(1).Date;

            var query = answerLogRepo.GetAll()
                .Where(ff => ff.CreationTime >= input.From && ff.CreationTime <= input.To)
                .WhereIf(input.DataSetId != null, ff => ff.DataSetId == input.DataSetId)
                .GroupBy(ff => ff.CreatorUserId)
                .Select(ff => new AnswersCountsOutput
                {
                    UserId = ff.Key.Value,
                    Count = ff.Count()
                });

            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                var user = usersRepo.Get(item.UserId);
                item.Name = user.Name;
                item.Surname = user.Surname;
            }

            return result;
        }

        public async Task<IEnumerable<AnswersCountsOutput>> Scoreboard(ScoreboardInput input)
        {
            var query = answerLogRepo.GetAll()
                .WhereIf(input.From != null, ff => ff.CreationTime >= input.From)
                .WhereIf(input.DataSetId != null, ff => ff.DataSetId == input.DataSetId)
                .GroupBy(ff => ff.CreatorUserId)
                .Select(ff => new AnswersCountsOutput
                {
                    UserId = ff.Key.Value,
                    Count = ff.Count()
                })
                .OrderByDescending(ff => ff.Count);

            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                var user = usersRepo.Get(item.UserId);
                item.Name = user.Name;
                item.Surname = user.Surname;
            }

            return result;
        }
    }
}

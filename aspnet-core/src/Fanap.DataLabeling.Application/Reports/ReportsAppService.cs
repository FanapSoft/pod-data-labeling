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
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;

namespace Fanap.DataLabeling.Reports
{
    public class DatasetReportOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TotalBudget { get; set; }
        public Guid? RandomItemId { get; set; }
        public LabelingStatus LabelingStatus { get; set; }
        public long ItemsCount { get; set; }
        public int AnswerBudgetCountPerUser { get; set; }
        public DateTime CreationTime { get; internal set; }
    }
    public class ReportsAppService : DataLabelingAppServiceBase
    {
        private readonly IRepository<DatasetItem, Guid> datasetItemRepo;
        private readonly IRepository<Dataset, Guid> datasetRepo;
        private readonly IRepository<AnswerLog, Guid> answerLogRepo;
        private readonly IRepository<User, long> usersRepo;

        public ReportsAppService(IRepository<DatasetItem, Guid> datasetItemRepo, IRepository<Dataset, Guid> datasetRepo, IRepository<AnswerLog, Guid> answerLogRepo, IRepository<User, long> usersRepo)
        {
            this.datasetItemRepo = datasetItemRepo;
            this.datasetRepo = datasetRepo;
            this.answerLogRepo = answerLogRepo;
            this.usersRepo = usersRepo;
        }

        [AbpAuthorize]
        [HttpGet]
        public async Task<IEnumerable<AnswersCountsOutput>> AnswersCountsTrend(AnswersCountsInput input)
        {
            if (input.From == null)
                input.From = DateTime.UtcNow.Date.AddDays(-7);
            if (input.To == null)
                input.To = DateTime.UtcNow.AddDays(1).Date;

            var query = answerLogRepo.GetAll()
                .Where(ff => ff.CreationTime >= input.From && ff.CreationTime <= input.To)
                .WhereIf(input.DataSetId != null, ff => ff.DataSetId == input.DataSetId)
                .WhereIf(input.UserId != null, ff => ff.CreatorUserId == input.UserId)
                .GroupBy(ff => new { ff.CreatorUserId, ff.CreationTime.Date })
                .Select(ff => new AnswersCountsOutput
                {
                    UserId = ff.Key.CreatorUserId.Value,
                    Date = ff.Key.Date,
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

        [HttpGet]
        public async Task<IEnumerable<AnswersCountsOutput>> Scoreboard(ScoreboardInput input)
        {
            if (input.MaxResultCount == 0)
                input.MaxResultCount = 10;
            var query = answerLogRepo.GetAll()
                .WhereIf(input.From != null, ff => ff.CreationTime >= input.From)
                .WhereIf(input.DataSetId != null, ff => ff.DataSetId == input.DataSetId)
                .GroupBy(ff => ff.CreatorUserId)
                .Select(ff => new AnswersCountsOutput
                {
                    UserId = ff.Key.Value,
                    Count = ff.Count()
                })
                .OrderByDescending(ff => ff.Count).Take(input.MaxResultCount);

            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                var user = usersRepo.Get(item.UserId);
                item.Name = user.Name;
                item.Surname = user.Surname;
            }

            return result;
        }

        [HttpGet]
        public async Task<PagedResultDto<DatasetReportOutput>> DataSets(PagedResultRequestDto input)
        {
            if (input.MaxResultCount == 0)
                input.MaxResultCount = 10;
            var totalCount = datasetRepo.Count();

            var query = await datasetRepo.GetAll().Select(ff => new DatasetReportOutput
            {
                Id = ff.Id,
                Description = ff.Description,
                Name = ff.Name,
                TotalBudget = ff.TotalBudget,
                CreationTime = ff.CreationTime,
                LabelingStatus = ff.LabelingStatus,
                AnswerBudgetCountPerUser = ff.AnswerBudgetCountPerUser
            }).OrderByDescending(ff => ff.CreationTime).Take(input.MaxResultCount).ToListAsync();

            foreach (var item in query)
            {
                item.ItemsCount = datasetItemRepo.Count(ff => ff.DatasetID == item.Id);
                item.RandomItemId = datasetItemRepo.GetAll()
                    .Where(ff => ff.DatasetID == item.Id)
                    .OrderBy(ff => Guid.NewGuid()).FirstOrDefault()?.Id;
            }
            return new PagedResultDto<DatasetReportOutput>(totalCount, query);
        }
    }
}

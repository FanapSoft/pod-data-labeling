using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Fanap.DataLabeling.Datasets;
using Fanap.DataLabeling.Labels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    [AbpAuthorize]
    public class DataSetsAppService : AsyncCrudAppService<Dataset, DatasetDto, Guid>, IDataSetsAppService
    {
        private readonly IRepository<AnswerLog, Guid> answerRepo;
        private readonly IRepository<AnswerOption, Guid> answerOptionsRepo;
        private readonly IRepository<Label, Guid> labelRepo;
        private readonly IBackgroundJobManager backgroundJobManager;

        public DataSetsAppService(
            IRepository<AnswerLog, Guid> answerRepo,
            IRepository<Dataset, Guid> repository,
            IRepository<AnswerOption, Guid> answerOptionsRepo,
            IRepository<Label, Guid> labelRepo,
            IBackgroundJobManager backgroundJobManager) : base(repository)
        {
            this.answerRepo = answerRepo;
            this.answerOptionsRepo = answerOptionsRepo;
            this.labelRepo = labelRepo;
            this.backgroundJobManager = backgroundJobManager;
        }

        public async override Task<DatasetDto> UpdateAsync(DatasetDto input)
        {
            answerOptionsRepo.Delete(ff => ff.DataSetId == input.Id);

            labelRepo.Delete(ff => ff.DatasetId == input.Id);

            CurrentUnitOfWork.SaveChanges();

            var res = await base.UpdateAsync(input);

            return res;
        }

        protected override IQueryable<Dataset> CreateFilteredQuery(PagedAndSortedResultRequestDto input)
        {
            return base.CreateFilteredQuery(input).Include(ff => ff.AnswerOptions);
        }
        public async Task<ImportOutput> Import(ImportInput input)
        {
            if (!Directory.Exists(input.FolderPath))
                throw new UserFriendlyException("Path does not exists in local machine.");

            var itemsSourcePath = Path.Join(input.FolderPath, "original images");
            var processedPath = Path.Join(input.FolderPath, "processed_images");

            if (!Directory.Exists(itemsSourcePath))
                throw new UserFriendlyException("original images path not found");
            if (!Directory.Exists(processedPath))
                throw new UserFriendlyException("processed_images path not found");

            var jobId = backgroundJobManager.Enqueue<DataSetImportJob, ImportInput>(input);

            return new ImportOutput()
            {
                JobId = jobId,
                DataSetId = input.DataSetId
            };
        }
        public async Task<DataSetStatisticsOutput> Stats(DataSetStatisticsInput input)
        {
            var dataSetIds = await answerRepo.GetAll()
                .WhereIf(input.UserId != null, ff => ff.CreatorUserId == input.UserId)
                .Select(ff => ff.DataSetId).ToListAsync();

            var query = await Repository
                .GetAll()
                .WhereIf(input.UserId != null, ff => dataSetIds.Contains(ff.Id))
                .WhereIf(input.LabelingStatus != null, ff => ff.LabelingStatus == input.LabelingStatus.Value)
                .CountAsync();

            return new DataSetStatisticsOutput { TotalCount = query };
        }
    }
}

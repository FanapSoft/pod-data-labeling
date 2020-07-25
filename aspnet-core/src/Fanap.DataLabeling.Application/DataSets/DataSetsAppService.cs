using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    [AbpAuthorize]
    public class DataSetsAppService : AsyncCrudAppService<Dataset, DatasetDto, Guid>, IDataSetsAppService
    {
        private readonly IBackgroundJobManager backgroundJobManager;

        public DataSetsAppService(IRepository<Dataset, Guid> repository, IBackgroundJobManager backgroundJobManager) : base(repository)
        {
            this.backgroundJobManager = backgroundJobManager;
        }

        public async Task<ImportOutput> Import(ImportInput input)
        {
            var jobId = backgroundJobManager.Enqueue<DataSetImportJob, ImportInput>(input);

            return new ImportOutput()
            {
                JobId = jobId,
                DataSetId = input.DataSetId
            };
        }
    }
}

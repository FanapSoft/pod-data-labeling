using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public class DataSetImportJob : BackgroundJob<ImportInput>, ITransientDependency
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Dataset, Guid> dataSetRepo;
        private readonly IRepository<DatasetItem, Guid> itemRepo;

        public DataSetImportJob(IRepository<User, long> userRepository, IRepository<Dataset, Guid> dataSetRepo, IRepository<DatasetItem, Guid> itemRepo)
        {
            _userRepository = userRepository;
            this.dataSetRepo = dataSetRepo;
            this.itemRepo = itemRepo;
        }

        [UnitOfWork]
        public override void Execute(ImportInput input)
        {
            var foundDateset = dataSetRepo.Get(input.DataSetId);
            var allfiles = Directory.GetFiles(input.FolderPath, "*.*", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {
                var fileInfo = new FileInfo(file);
                var item = new DatasetItem();
                item.FilePath = fileInfo.FullName;
                item.Label = new Labels.Label
                {
                    Name = fileInfo.Directory.Name,
                    DatasetId = foundDateset.Id
                };
                item.FileExtension = Path.GetExtension(fileInfo.FullName);
                item.FileSize = fileInfo.Length;

                itemRepo.Insert(item);
            }

        }
    }

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

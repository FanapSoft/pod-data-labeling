using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Datasets;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System;
using System.IO;

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
            
            foundDateset.ItemsSourcePath = input.FolderPath;
            
            
            var allfiles = Directory.GetFiles(input.FolderPath, "*.*", SearchOption.AllDirectories);
            foundDateset.ItemsSourcePath = input.FolderPath;
            foreach (var file in allfiles)
            {
                var fileInfo = new FileInfo(file);
                var item = new DatasetItem();
                item.DatasetID = input.DataSetId;
                item.Name = fileInfo.Name;
                item.FilePath = fileInfo.FullName;
                item.FileName = fileInfo.Name;
                item.Label = new Labels.Label
                {
                    Name = fileInfo.Directory.Name,
                    DatasetId = foundDateset.Id
                };
                item.FileExtension = Path.GetExtension(fileInfo.FullName);
                item.FileSize = fileInfo.Length;
                
                itemRepo.Insert(item);
            }
            CurrentUnitOfWork.SaveChanges();
        }
    }
}

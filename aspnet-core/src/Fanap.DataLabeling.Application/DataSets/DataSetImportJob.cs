using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Datasets;
using Fanap.DataLabeling.Labels;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System;
using System.IO;
using System.Linq;

namespace Fanap.DataLabeling.DataSets
{
    public class DataSetImportJob : BackgroundJob<ImportInput>, ITransientDependency
    {
        private readonly IRepository<Label, Guid> labelRepo;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Dataset, Guid> dataSetRepo;
        private readonly IRepository<DatasetItem, Guid> itemRepo;

        public DataSetImportJob(
            IRepository<Label, Guid> labelRepo,
            IRepository<User, long> userRepository,
            IRepository<Dataset, Guid> dataSetRepo,
            IRepository<DatasetItem, Guid> itemRepo)
        {
            this.labelRepo = labelRepo;
            _userRepository = userRepository;
            this.dataSetRepo = dataSetRepo;
            this.itemRepo = itemRepo;
        }

        [UnitOfWork]
        public override void Execute(ImportInput input)
        {
            var foundDateset = dataSetRepo.Get(input.DataSetId);
            var itemsSourcePath = Path.Join(input.FolderPath, "original images");
            foundDateset.ItemsSourcePath = itemsSourcePath;
            var processedPath = Path.Join(input.FolderPath, "processed_images");
            foundDateset.ProcessedItemsSourcePath = processedPath;

            var allProcessedfiles = Directory.GetFiles(processedPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in allProcessedfiles)
            {
                var fileInfo = new FileInfo(file);
                var item = new DatasetItem();
                item.DatasetID = input.DataSetId;
                item.Name = fileInfo.Name;
                item.FilePath = fileInfo.FullName;
                item.FileName = fileInfo.Name;
                item.LabelId = GetOrCreateLabelId(foundDateset.Id, fileInfo.Directory.Name);
                item.FileExtension = Path.GetExtension(fileInfo.FullName);
                item.FileSize = fileInfo.Length;

                itemRepo.Insert(item);
            }
            CurrentUnitOfWork.SaveChanges();
        }

        private Guid GetOrCreateLabelId(Guid datasetId, string name)
        {
            var found = labelRepo.GetAll().SingleOrDefault(ff => ff.DatasetId == datasetId && ff.Name == name);
            if (found == null)
            {
                var label = labelRepo.Insert(new Labels.Label
                {
                    Name = name,
                    DatasetId = datasetId
                });
                CurrentUnitOfWork.SaveChanges();
                return label.Id;
            }
            else
            {
                return found.Id;
            }
        }
    }
}

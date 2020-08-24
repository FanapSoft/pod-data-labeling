using Abp.UI;
using Fanap.DataLabeling.DataSets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fanap.DataLabeling.Tests.Datasets
{
    public class DataSetsAppService_Tests : DataLabelingTestBase
    {
        private readonly IDataSetsAppService _appService;

        public DataSetsAppService_Tests()
        {
            _appService = Resolve<IDataSetsAppService>();
        }

        [Fact]
        public async Task DatasetImport_FolderPath_Empty_Exception()
        {
            // act
            // assert
            await Assert.ThrowsAnyAsync<Exception>(() =>
            {
                return _appService.Import(new ImportInput { FolderPath = "", DataSetId = Guid.NewGuid() });
            });
        }

        [Fact]
        public async Task DatasetImport_FolderPath_NotLocal_Exception()
        {
            // act
            // assert
            await Assert.ThrowsAnyAsync<UserFriendlyException>(() =>
            {
                return _appService.Import(new ImportInput { FolderPath = "//Test/test/test", DataSetId = Guid.NewGuid() });
            });
        }

        [Fact]
        public async Task DatasetImport_AlreadyImported_Exception()
        {
            // arrange
            var setId = Guid.NewGuid();
            UsingDbContext(ff =>
            {
                ff.Datasets.Add(new DataLabeling.Datasets.Dataset
                {
                    Id = setId,
                    Name = "test",
                    ItemsSourcePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    IsActive = true,
                    Type = DataLabeling.Datasets.DatasetType.Images
                });

                ff.SaveChanges();
            });
            // act
            // assert
            await Assert.ThrowsAnyAsync<UserFriendlyException>(() =>
            {
                return _appService.Import(new ImportInput
                {
                    FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    DataSetId = setId
                });
            });
        }

    }
}

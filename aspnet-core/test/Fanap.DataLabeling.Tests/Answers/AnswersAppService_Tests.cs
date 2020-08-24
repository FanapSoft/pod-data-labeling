using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Application.Services.Dto;
using Fanap.DataLabeling.Users;
using Fanap.DataLabeling.Users.Dto;
using Fanap.DataLabeling.DataSets;
using System;
using Abp.UI;

namespace Fanap.DataLabeling.Tests.Answers
{
    public class AnswersAppService_Tests : DataLabelingTestBase
    {
        private readonly IAnswersAppService _appService;

        public AnswersAppService_Tests()
        {
            _appService = Resolve<IAnswersAppService>();
        }

        [Fact]
        public async Task SubmitAnswer_DataSetNotExists_Exception()
        {
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<UserFriendlyException>(() =>
            {
                return _appService.SubmitAnswer(new SubmitAnswerInput
                {
                    AnswerIndex = 0,
                    DataSetId = Guid.NewGuid(),
                    DataSetItemId = Guid.NewGuid()
                });
            });

        }
        [Fact]
        public async Task SubmitAnswer_DataSetIsNotActive_Exception()
        {
            var setId = Guid.NewGuid();
            //Arrange
            UsingDbContext(ff =>
            {
                ff.Datasets.Add(new DataLabeling.Datasets.Dataset
                {

                    Id = setId,
                    Name = "test",
                    IsActive = false,
                    LabelingStatus = DataLabeling.Datasets.LabelingStatus.None,
                    Type = DataLabeling.Datasets.DatasetType.Images
                });
            });
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<Exception>(() =>
            {
                return _appService.SubmitAnswer(new SubmitAnswerInput
                {
                    AnswerIndex = 0,
                    DataSetId = setId,
                    DataSetItemId = Guid.NewGuid()
                });
            });
        }
        [Fact]
        public async Task SubmitAnswer_DataSetIsNotInLabelingStatus_Exception()
        {
            var setId = Guid.NewGuid();
            //Arrange
            UsingDbContext(ff =>
            {
                ff.Datasets.Add(new DataLabeling.Datasets.Dataset
                {

                    Id = setId,
                    Name = "test",
                    IsActive = true,
                    LabelingStatus = DataLabeling.Datasets.LabelingStatus.None,
                    Type = DataLabeling.Datasets.DatasetType.Images
                });
            });
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<Exception>(() =>
            {
                return _appService.SubmitAnswer(new SubmitAnswerInput
                {
                    AnswerIndex = 0,
                    DataSetId = setId,
                    DataSetItemId = Guid.NewGuid()
                });
            });
        }
        [Fact]
        public async Task SubmitAnswer_DataSetItemDoesNotExists_Exception()
        {
            var setId = Guid.NewGuid();
            //Arrange
            UsingDbContext(ff =>
            {
                ff.Datasets.Add(new DataLabeling.Datasets.Dataset
                {

                    Id = setId,
                    Name = "test",
                    IsActive = true,
                    LabelingStatus = DataLabeling.Datasets.LabelingStatus.Started,
                    Type = DataLabeling.Datasets.DatasetType.Images
                });
            });
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<Exception>(() =>
            {
                return _appService.SubmitAnswer(new SubmitAnswerInput
                {
                    AnswerIndex = 0,
                    DataSetId = setId,
                    DataSetItemId = Guid.NewGuid()
                });
            });
        }
        [Fact]
        public async Task SubmitAnswer_IsIgnore_IgnoreReasonEmpty_Exception()
        {
            var setId = Guid.NewGuid();
            var setItemId = Guid.NewGuid();
            //Arrange
            UsingDbContext(ff =>
            {
                ff.Datasets.Add(new DataLabeling.Datasets.Dataset
                {

                    Id = setId,
                    Name = "test",
                    IsActive = true,
                    LabelingStatus = DataLabeling.Datasets.LabelingStatus.Started,
                    Type = DataLabeling.Datasets.DatasetType.Images
                });
                ff.DatasetItems.Add(new DataLabeling.Datasets.DatasetItem
                {
                    Id = setItemId,
                    Content = "",
                    Type = DataLabeling.Datasets.DatasetItemType.File,
                    Name = "test"
                });
            });
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<Exception>(() =>
            {
                return _appService.SubmitAnswer(new SubmitAnswerInput
                {
                    AnswerIndex = 0,
                    DataSetId = setId,
                    Ignored = true,
                    IgnoreReason = "",
                    DataSetItemId = setItemId
                });
            });
        }
    }
}

using Abp.UI;
using Fanap.DataLabeling.DataSets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fanap.DataLabeling.Tests.Questions
{
    public class QuestionsAppService_Tests : DataLabelingTestBase
    {
        private readonly IQuestionsAppService _appService;

        public QuestionsAppService_Tests()
        {
            _appService = Resolve<IQuestionsAppService>();
        }

        public async Task GetQuestion_DataSetNotExists_Exception()
        {
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<UserFriendlyException>(() =>
            {
                return _appService.GetQuestion(new GetQuestionInput
                {
                    DataSetId = Guid.NewGuid(),
                    DataSetItemId = Guid.NewGuid()
                });
            });
        }
        public async Task GetQuestion_DataSetItemNotExists_Exception()
        {
            var setId = Guid.NewGuid();
            // Arrange
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
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<UserFriendlyException>(() =>
            {
                return _appService.GetQuestion(new GetQuestionInput
                {
                    DataSetId = setId,
                    DataSetItemId = Guid.NewGuid()
                });
            });
        }
        [Fact]
        public async Task GetQuestion_DataSetQuestionTypeIsText_QuestionTemplateEmpty_Exception()
        {
            var setId = Guid.NewGuid();
            var setItemId = Guid.NewGuid();
            // Arrange
            UsingDbContext(ff =>
            {

                ff.Datasets.Add(new DataLabeling.Datasets.Dataset
                {
                    Id = setId,
                    Name = "test",
                    ItemsSourcePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    IsActive = true,
                    Type = DataLabeling.Datasets.DatasetType.Images,
                    QuestionTemplate = "",
                    QuestionType = DataLabeling.Datasets.QuestionType.Text,
                    AnswerOptions = new List<DataLabeling.Datasets.AnswerOption>() {
                       new DataLabeling.Datasets.AnswerOption{
                       Id = Guid.NewGuid(),
                       DataSetId = setId,
                       Title = "test",
                       }


                    }
                });

                ff.DatasetItems.Add(new DataLabeling.Datasets.DatasetItem
                {
                    Id = setItemId,
                    DatasetID = setId,
                });

                ff.SaveChanges();

            });
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<UserFriendlyException>(() =>
            {
                return _appService.GetQuestion(new GetQuestionInput
                {
                    DataSetId = setId,
                    DataSetItemId = Guid.NewGuid()
                });
            });
        }

        [Fact]
        public async Task GetQuestion_DataSetQuestionTypeIsAudioVideo_QuestionSrcEmpty_Exception()
        {
            var setId = Guid.NewGuid();
            var setItemId = Guid.NewGuid();
            // Arrange
            UsingDbContext(ff =>
            {

                ff.Datasets.Add(new DataLabeling.Datasets.Dataset
                {
                    Id = setId,
                    Name = "test",
                    ItemsSourcePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    IsActive = true,
                    Type = DataLabeling.Datasets.DatasetType.Images,
                    QuestionTemplate = "ad",
                    QuestionSrc = "",
                    QuestionType = DataLabeling.Datasets.QuestionType.Video,
                    AnswerOptions = new List<DataLabeling.Datasets.AnswerOption>() {
                       new DataLabeling.Datasets.AnswerOption{
                       Id = Guid.NewGuid(),
                       DataSetId = setId,
                       Title = "test",
                       }


                    }
                });

                ff.DatasetItems.Add(new DataLabeling.Datasets.DatasetItem
                {
                    Id = setItemId,
                    DatasetID = setId,
                });

                ff.SaveChanges();

            });
            // Act
            // Assert
            await Assert.ThrowsAnyAsync<UserFriendlyException>(() =>
            {
                return _appService.GetQuestion(new GetQuestionInput
                {
                    DataSetId = setId,
                    DataSetItemId = Guid.NewGuid()
                });
            });
        }

    }
}

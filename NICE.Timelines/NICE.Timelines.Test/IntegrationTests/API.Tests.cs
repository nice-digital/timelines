using System;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;
using NICE.Timelines.DB.Services;
using NICE.Timelines.Services;
using NICE.Timelines.Test.Infrastructure;
using Shouldly;
using Xunit;

namespace NICE.Timelines.Test.IntegrationTests
{
    public class API : TestBase
    {
        [Fact]
        public async void CanGetTaskDataFromClickUp()
        {
            //Arrange
            var clickUpConfig = GetClickUpConfig();
            var testConfig = GetTestConfig();
            var context = new TimelinesContext();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var dbService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            var clickUpService = new ClickUpService(clickUpConfig, context, dbService, conversionService, CreateHttpClientFactory(), Mock.Of<ILogger<ClickUpService>>());

            //Act
            var response = await clickUpService.ReturnClickUpData<ClickUpTasks>(clickUpConfig.GetKeyDateTasks, testConfig.ListId);

            //Assert
            response.Tasks.ShouldNotBeEmpty();
            response.Tasks.First().ClickUpTaskId.ShouldBe(testConfig.TaskId);
            response.Tasks.First().DueDateSecondsSinceUnixEpochAsString.ShouldBe("1637726400000");
            response.Tasks.First().Folder.Id.ShouldBe(testConfig.FolderId);
            response.Tasks.First().List.Id.ShouldBe(testConfig.ListId);
            response.Tasks.First().Name.ShouldBe("ERG Report & papers sent to C&Cs for Technical engagement (cc appraisals editors and PIP)");
            response.Tasks.First().Space.Id.ShouldBe(testConfig.SpaceId);

        }

        [Fact]
        public async void CanGetFolderDataFromClickUp()
        {
            //Arrange
            var clickUpConfig = GetClickUpConfig();
            var testConfig = GetTestConfig();
            var context = new TimelinesContext();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var dbService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            var clickUpService = new ClickUpService(clickUpConfig, context, dbService, conversionService, CreateHttpClientFactory(), Mock.Of<ILogger<ClickUpService>>());

            //Act
            var response = await clickUpService.ReturnClickUpData<ClickUpFolders>(clickUpConfig.GetFolders, testConfig.SpaceId);

            //Assert
            response.Folders.ShouldNotBeEmpty();
            response.Folders[2].Id.ShouldBe(testConfig.FolderId);
            response.Folders[2].Name.ShouldBe("Test");
            response.Folders[2].Lists.Count.ShouldBe(9);
        }

        [Fact]
        public async void CanGetListDataFromClickUp()
        {
            //Arrange
            var clickUpConfig = GetClickUpConfig();
            var testConfig = GetTestConfig();
            var context = new TimelinesContext();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var dbService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            var clickUpService = new ClickUpService(clickUpConfig, context, dbService, conversionService, CreateHttpClientFactory(), Mock.Of<ILogger<ClickUpService>>());

            //Act
            var response = await clickUpService.ReturnClickUpData<ClickUpLists>(clickUpConfig.GetLists, testConfig.FolderId);

            //Assert
            response.Lists.ShouldNotBeEmpty();
            response.Lists.First().Id.ShouldBe(testConfig.ListId);
            response.Lists.First().Name.ShouldBe("674_Appraisal project timeline");

            response.Lists.First().Folder.Id.ShouldBe(testConfig.FolderId);
            response.Lists.First().Folder.Name.ShouldBe("Test");
            response.Lists.First().Folder.Lists.ShouldBeNull();
        }

        [Theory]
        [InlineData(Constants.ClickUp.Fields.ACID, "ACID")]
        [InlineData(Constants.ClickUp.Fields.PhaseId, "Phase ID")]
        [InlineData(Constants.ClickUp.Fields.OrderInPhase, "Order in phase")]
        public async void CanGetMandatoryCustomFieldFromClickUp(string customFieldId, string customFieldName)
        {
            //Arrange
            var clickUpConfig = GetClickUpConfig();
            var testConfig = GetTestConfig();
            var context = new TimelinesContext();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var dbService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            var clickUpService = new ClickUpService(clickUpConfig, context, dbService, conversionService, CreateHttpClientFactory(), Mock.Of<ILogger<ClickUpService>>());

            //Act
            var response = await clickUpService.ReturnClickUpData<ClickUpTasks>(clickUpConfig.GetKeyDateTasks, testConfig.ListId);
            response.Tasks.ShouldNotBeEmpty();

            var customField = response.Tasks.First().CustomFields.First(field =>
                field.FieldId.Equals(customFieldId,
                    StringComparison.InvariantCultureIgnoreCase));

            //Assert
            customField.FieldId.ShouldBe(customFieldId);
            customField.Name.ShouldBe(customFieldName);
            customField.Value.ValueKind.ShouldBe(JsonValueKind.String);
        }

        [Theory]
        [InlineData(Constants.ClickUp.Fields.TaskTypeId, "Task type ID")]
        [InlineData(Constants.ClickUp.Fields.CompletedDate, "Date completed")]
        [InlineData(Constants.ClickUp.Fields.KeyInfo, "Key info")]
        [InlineData(Constants.ClickUp.Fields.KeyDate, "Key date")]
        [InlineData(Constants.ClickUp.Fields.MasterSchedule, "Master schedule")]
        public async void CanGetOptionalCustomFieldFromClickUp(string customFieldId, string customFieldName)
        {
            //Arrange
            var clickUpConfig = GetClickUpConfig();
            var testConfig = GetTestConfig();
            var context = new TimelinesContext();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var dbService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            var clickUpService = new ClickUpService(clickUpConfig, context, dbService, conversionService, CreateHttpClientFactory(), Mock.Of<ILogger<ClickUpService>>());

            //Act
            var response = await clickUpService.ReturnClickUpData<ClickUpTasks>(clickUpConfig.GetKeyDateTasks, testConfig.ListId);
            response.Tasks.ShouldNotBeEmpty();

            var customField = response.Tasks.First().CustomFields.First(field =>
                field.FieldId.Equals(customFieldId,
                    StringComparison.InvariantCultureIgnoreCase));

            //Assert
            customField.FieldId.ShouldBe(customFieldId);
            customField.Name.ShouldBe(customFieldName);
            (customField.Value.ValueKind == JsonValueKind.Undefined || customField.Value.ValueKind == JsonValueKind.String).ShouldBeTrue();
        }
    }
}

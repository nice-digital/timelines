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
        }
    }
}

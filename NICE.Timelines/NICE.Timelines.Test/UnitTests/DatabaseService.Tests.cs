using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;
using NICE.Timelines.DB.Services;
using NICE.Timelines.Test.Infrastructure;
using Shouldly;
using Xunit;

namespace NICE.Timelines.Test.UnitTests
{
    public class DatabaseServiceTests : TestBase
    {
        [Fact]
        public void SavesNewTaskToDatabaseCorrectly()
        {
            //Arrange
            var context = new TimelinesContext(GetContextOptions());
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var databaseService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();

            //Act
            databaseService.SaveOrUpdateTimelineTask(task);
            context.SaveChanges();

            //Assert
            context.TimelineTasks.Count().ShouldBe(1);
        }

        [Fact]
        public void TaskIsNotSavedIfACIDAndPhaseIdIsMissing()
        {
            //Arrange
            var context = new TimelinesContext(GetContextOptions());
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var databaseService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task-missing-acid-and-phaseid.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();

            //Act
            databaseService.SaveOrUpdateTimelineTask(task);
            context.SaveChanges();

            //Assert
            context.TimelineTasks.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdatesTaskInDatabaseCorrectly()
        {
            //Arrange
            var context = new TimelinesContext(GetContextOptions());
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var databaseService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            databaseService.SaveOrUpdateTimelineTask(task);
            context.SaveChanges();

            task.DueDateSecondsSinceUnixEpochAsString = "1625816820555";

            //Act
            databaseService.SaveOrUpdateTimelineTask(task);
            context.SaveChanges();

            //Assert
            context.TimelineTasks.Count().ShouldBe(1);
            context.TimelineTasks.First().DueDate.ShouldBe(new DateTime(2021, 7, 9, 8, 47, 0, 555));
        }

        [Fact]
        public void CorrectTasksAreDeleted()
        {
            //Arrange
            var Acid = 123;
            var taskId = "246";

            var context = new TimelinesContext(GetContextOptions());
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());
            var databaseService = new DatabaseService(context, conversionService, Mock.Of<ILogger<DatabaseService>>());

            AddTimelineTask(context, Acid, 1, 1, "desc", "1", "1", "1", taskId, DateTime.Now, DateTime.Now);
            AddTimelineTask(context, Acid, 1, 1, "desc", "1", "1", "1", "2", DateTime.Now, DateTime.Now);
            AddTimelineTask(context, 2, 1, 1, "desc", "1", "1", "1", taskId, DateTime.Now, DateTime.Now);

            //Act
            databaseService.DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(Acid, new List<string>() { taskId });
            context.SaveChanges();

            //Assert
            context.TimelineTasks.Count().ShouldBe(2);
        }
    }
}
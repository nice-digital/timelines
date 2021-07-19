using System;
using System.Collections.Generic;
using System.Linq;
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
        public void CorrectTasksAreDeleted()
        {
            //Arrange
            var Acid = 123;
            var taskId = "246";

            var context = new TimelinesContext(GetContextOptions());
            var conversionService = new ConversionService();
            var databaseService = new DatabaseService(context, conversionService);

            AddTimelineTask(context, Acid, 1, 1, "1", "1", "1", taskId, DateTime.Now, DateTime.Now);
            AddTimelineTask(context, Acid, 1, 1, "1", "1", "1", "2", DateTime.Now, DateTime.Now);
            AddTimelineTask(context, 2, 1, 1, "1", "1", "1", taskId, DateTime.Now, DateTime.Now);

            //Act
            databaseService.DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(Acid, new List<string>() { taskId });
            context.SaveChanges();

            //Assert
            context.TimelineTasks.Count().ShouldBe(2);
        }
    }
}
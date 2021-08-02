using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Services;
using Shouldly;
using Xunit;

namespace NICE.Timelines.Test.UnitTests
{
    public class ConversionServiceTests
    {
        [Fact]
        public void CanGetACID()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var ACID = conversionService.GetACID(task);

            //Assert
            ACID.ShouldBe(674);
        }

        [Fact]
        public void CanGetTaskType()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var taskTypeId = conversionService.GetTaskTypeId(task);

            //Assert
            taskTypeId.ShouldBe(7);
        }

        [Fact]
        public void CanGetPhase()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var phaseId = conversionService.GetPhaseId(task);

            //Assert
            phaseId.ShouldBe(12);
        }
        
        [Fact]
        public void CanGetDateCompleted()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var actualDate = conversionService.GetActualDate(task);

            //Assert
            actualDate.ShouldBe(new DateTime(2021, 7, 24, 4, 0, 0, 0));
        }

        [Fact]
        public void CanGetDueDate()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var dueDate = conversionService.GetDueDate(task);

            //Assert
            dueDate.ShouldBe(new DateTime(2021, 6, 25, 12, 11, 18, 126));
        }
    }
}

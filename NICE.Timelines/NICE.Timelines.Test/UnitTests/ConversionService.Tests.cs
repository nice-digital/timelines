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
            var ACID = conversionService.GetIntegerCustomField(task, Constants.ClickUp.Fields.ACID, true, "acid");

            //Assert
            ACID.ShouldBe(674);
        }

        [Fact]
        public void NoACIDShouldReturn0()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task-missing-acid-and-phaseid.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var ACID = conversionService.GetIntegerCustomField(task, Constants.ClickUp.Fields.ACID, true, "acid");

            //Assert
            ACID.ShouldBe(0);
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
            var taskTypeId = conversionService.GetIntegerCustomField(task, Constants.ClickUp.Fields.TaskTypeId, false);

            //Assert
            taskTypeId.ShouldBe(7);
        }

        [Fact]
        public void NoTaskTypeShouldReturn0()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task-missing-acid-and-phaseid.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var taskTypeId = conversionService.GetIntegerCustomField(task, Constants.ClickUp.Fields.TaskTypeId, false);

            //Assert
            taskTypeId.ShouldBe(0);
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
            var phaseId = conversionService.GetIntegerCustomField(task, Constants.ClickUp.Fields.PhaseId, true, "phaseId");

            //Assert
            phaseId.ShouldBe(12);
        }

        [Fact]
        public void NoPhaseIdShouldReturn0()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task-missing-acid-and-phaseid.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var phaseId = conversionService.GetIntegerCustomField(task, Constants.ClickUp.Fields.PhaseId, true, "phaseId");

            //Assert
            phaseId.ShouldBe(0);
        }

        [Fact]
        public void CanGetOrderInPhase()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var orderInPhase = conversionService.GetIntegerCustomField(task, Constants.ClickUp.Fields.OrderInPhase, true, "orderInPhase");

            //Assert
            orderInPhase.ShouldBe(4);
        }

        [Fact]
        public void NoOrderInPhaseShouldReturn0()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "single-task-missing-acid-and-phaseid.json");
            string json = File.ReadAllText(path);

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService(Mock.Of<ILogger<ConversionService>>());

            //Act
            var orderInPhase = conversionService.GetIntegerCustomField(task, Constants.ClickUp.Fields.OrderInPhase, true, "orderInPhase");

            //Assert
            orderInPhase.ShouldBe(0);
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
            var actualDate = conversionService.GetDateCustomField(task, Constants.ClickUp.Fields.CompletedDate);

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

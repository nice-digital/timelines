using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Services;
using NICE.Timelines.Test.Infrastructure;
using Shouldly;
using Xunit;

namespace NICE.Timelines.Test.UnitTests
{
    public class ConversionServiceTests
    {
        [Fact]
        public void CanGetACIDFromClickUpTask()
        {
            //Arrange
            string someJson = @"{
                ""tasks"":  [{
                    ""custom_fields"": [{
                        ""id"": ""d5e3053f-b7d3-4b61-a358-7b8327040af5"",
                        ""name"": ""ACID"",
                        ""value"": ""23""
                    }]
                }]
            }";

            var task = JsonSerializer.Deserialize<ClickUpTasks>(someJson).Tasks.First();
            var conversionService = new ConversionService();

            //Act
            var ACID = conversionService.GetACIDFromClickUpTask(task);

            //Assert
            ACID.ShouldBe(23);
        }
    }
}

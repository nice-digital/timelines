using System;
using System.Linq;
using System.Text.Json;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Services;
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
            string json = @"{
                ""tasks"":  [{
                    ""custom_fields"": [{
                        ""id"": ""d5e3053f-b7d3-4b61-a358-7b8327040af5"",
                        ""name"": ""ACID"",
                        ""value"": ""23""
                    }]
                }]
            }";

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService();

            //Act
            var ACID = conversionService.GetACIDFromClickUpTask(task);

            //Assert
            ACID.ShouldBe(23);
        }

        [Fact]
        public void CanGetTaskTypeFromClickUpTask()
        {
            //Arrange
            string json = @"{
                ""tasks"":  [{
                    ""custom_fields"": [{
                        ""id"": ""c92405fc-d4b5-487d-9a5c-ece99b72e728"",
                        ""name"": ""Task Type Id"",
                        ""value"": 0,
                        ""type_config"": {
                            ""options"": [{
                                ""id"": ""960b96c1-0d91-4227-a6ef-e251b073115a"",
                                ""name"": ""123""
                                }
                            ]
                        }
                   }]
                }]
            }";

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService();

            //Act
            var taskTypeId = conversionService.GetTaskTypeIdFromClickUpTask(task);

            //Assert
            taskTypeId.ShouldBe(123);
        }

        [Fact]
        public void CanGetPhaseFromClickUpTask()
        {
            //Arrange
            string json = @"{
                ""tasks"":  [{
                    ""custom_fields"": [{
                        ""id"": ""0aebe937-ee98-4d96-afc9-061a978e4a33"",
                        ""name"": ""Phase"",
                        ""value"": 0,
                        ""type_config"": {
                            ""options"": [{
                                ""id"": ""54397225-dfe4-491b-a60f-a02d73ca4a4e"",
                                ""name"": ""123""
                                }
                            ]
                        }
                   }]
                }]
            }";

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService();

            //Act
            var phase = conversionService.GetPhaseFromClickUpTask(task);

            //Assert
            phase.PhaseId.ShouldBe(123);
            phase.PhaseDescription.ShouldBe("123");
        }

        [Fact]
        public void CanGetActualDateFromClickUpTask()
        {
            //Arrange
            string json = @"{
                ""tasks"":  [{
                    ""custom_fields"": [{
                        ""id"": ""421ba5af-2c61-438f-978f-2dad19c7498c"",
                        ""name"": ""actual_date"",
                        ""value"": ""actual_date"",
                        ""type_config"": {
                            ""options"": [{
                                ""id"": ""54397225-dfe4-491b-a60f-a02d73ca4a4e"",
                                ""name"": ""actual_date"",
                                ""value"": ""1624619478126""
                                }
                            ]
                        }
                   }]
                }]
            }";

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService();

            //Act
            var actualDate = conversionService.GetActualDateFromClickUpTask(task);

            //Assert
            actualDate.ShouldBe(DateTime.Now);
        }

        [Fact]
        public void CanGetDueDateFromClickUpTask()
        {
            //Arrange
            string json = @"{
                ""tasks"":  [{
                    ""id"": ""421ba5af-2c61-438f-978f-2dad19c7498c"",
                    ""name"": ""due_date"",
                    ""value"": ""1624619478126""
                }]
            }";

            var task = JsonSerializer.Deserialize<ClickUpTasks>(json).Tasks.First();
            var conversionService = new ConversionService();

            //Act
            var dueDate = conversionService.GetDueDateFromClickUpTask(task);

            //Assert
            dueDate.ShouldBe(DateTime.Now);
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NICE.Timelines.Models;
using Shouldly;
using Xunit;

namespace NICE.Timelines.Test.UnitTests
{
    public class ClickUpServiceTests
    {
        [Fact]
        public void ClickUpFolder_IsDeserializedCorrectly()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "folders-in-space.json");
            string json = File.ReadAllText(path);

            //Act
            var deserialised = JsonSerializer.Deserialize<ClickUpFolders>(json);

            //Assert
            deserialised.Folders.First().Id.ShouldBe("457");
            deserialised.Folders.First().Name.ShouldBe("Updated Folder Name");
            deserialised.Folders.First().Lists.First().Id.ShouldBe("124");
            deserialised.Folders.First().Lists.First().Name.ShouldBe("Updated List Name");

            deserialised.Folders.Last().Id.ShouldBe("458");
            deserialised.Folders.Last().Name.ShouldBe("Second Folder Name");
            deserialised.Folders.Last().Lists.First().Id.ShouldBe("125");
            deserialised.Folders.Last().Lists.First().Name.ShouldBe("Second List");
        }

        [Fact]
        public void ListsInFolder_IsDeserializedCorrectly()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "lists-in-folder.json");
            string json = File.ReadAllText(path);

            //Act
            var deserialised = JsonSerializer.Deserialize<ClickUpLists>(json);

            //Assert
            deserialised.Lists.First().Id.ShouldBe("124");
            deserialised.Lists.First().Name.ShouldBe("Updated List Name");
            deserialised.Lists.First().Content.ShouldBe("Updated List Content");
            deserialised.Lists.First().Folder.Id.ShouldBe("456");
            deserialised.Lists.First().Folder.Name.ShouldBe("Folder Name");

            deserialised.Lists.Last().Id.ShouldBe("125");
            deserialised.Lists.Last().Name.ShouldBe("Second List");
            deserialised.Lists.Last().Content.ShouldBe("Second List Content");
            deserialised.Lists.Last().Folder.Id.ShouldBe("456");
            deserialised.Lists.Last().Folder.Name.ShouldBe("Folder Name");
        }

        [Fact]
        public void FolderlessList_IsDeserializedCorrectly()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "lists-not-in-folder.json");
            string json = File.ReadAllText(path);

            //Act
            var deserialised = JsonSerializer.Deserialize<ClickUpLists>(json);

            //Assert
            deserialised.Lists.First().Id.ShouldBe("124");
            deserialised.Lists.First().Name.ShouldBe("Updated List Name");
            deserialised.Lists.First().Content.ShouldBe("Updated List Content");
            deserialised.Lists.First().Folder.Id.ShouldBe("457");
            deserialised.Lists.First().Folder.Name.ShouldBe("hidden");

            deserialised.Lists.Last().Id.ShouldBe("125");
            deserialised.Lists.Last().Name.ShouldBe("Second List");
            deserialised.Lists.Last().Content.ShouldBe("Second List Content");
            deserialised.Lists.Last().Folder.Id.ShouldBe("457");
            deserialised.Lists.Last().Folder.Name.ShouldBe("hidden");
        }

        [Fact]
        public void ClickUpTasks_IsDeserializedCorrectly()
        {
            //Arrange
            string path = Path.Combine(Directory.GetCurrentDirectory(), "feeds", "tasks-in-list.json");
            string json = File.ReadAllText(path);

            //Act
            var deserialised = JsonSerializer.Deserialize<ClickUpTasks>(json);

            //Assert
            deserialised.Tasks.First().ClickUpTaskId.ShouldBe("9hx");
            deserialised.Tasks.First().Name.ShouldBe("New Task Name");
            deserialised.Tasks.First().DueDateSecondsSinceUnixEpochAsString.ShouldBe("1508369194377");
            
            deserialised.Tasks.First().CustomFields.First().FieldId.ShouldBe("0a52c486-5f05-403b-b4fd-c512ff05131c");
            deserialised.Tasks.First().CustomFields.First().Name.ShouldBe("My Number field");
            //deserialised.Tasks.First().CustomFields.First().Value.ShouldBe("23");

            deserialised.Tasks.First().CustomFields.Last().FieldId.ShouldBe("f4d2a20d-6759-4420-b853-222dbe2589d5");
            deserialised.Tasks.First().CustomFields.Last().Name.ShouldBe("My People");
            //deserialised.Tasks.First().CustomFields.Last().ClickUpTypeConfig.Options.First().Name.ShouldBe("23");

            deserialised.Tasks.First().Folder.Id.ShouldBe("456");
            deserialised.Tasks.First().List.Id.ShouldBe("123");
            deserialised.Tasks.First().Space.Id.ShouldBe("789");
        }
    }
}

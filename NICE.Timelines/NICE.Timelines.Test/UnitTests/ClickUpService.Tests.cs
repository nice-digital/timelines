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
        public void ResponseIsDeserializedCorrectly()
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
        public void ListsIsDeserializedCorrectly()
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
    }
}

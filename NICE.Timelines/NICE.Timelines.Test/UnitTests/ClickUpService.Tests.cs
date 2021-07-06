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
            deserialised.Folders.First().Name.ShouldBe("Folder one");
            deserialised.Folders.Last().Name.ShouldBe("Folder two");
        }
    }
}

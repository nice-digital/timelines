using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
    public class ClickUpList
    {
		[JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("folder")]
        public ClickUpFolder Folder { get; set; }
	}
}
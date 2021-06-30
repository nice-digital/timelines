using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
    public class ClickUpSpace
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

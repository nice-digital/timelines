using System.Text.Json;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
    public class ClickUpCustomField
    {
		[JsonPropertyName("id")]
        public string FieldId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public JsonElement Value { get; set; }

        [JsonPropertyName("type_config")]
        public ClickUpTypeConfig ClickUpTypeConfig { get; set; }
	}
}
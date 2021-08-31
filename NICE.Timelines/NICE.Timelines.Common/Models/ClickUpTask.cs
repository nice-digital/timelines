using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Common.Models
{
    public class ClickUpId
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
    public class ClickUpTask
    {
		[JsonPropertyName("id")]
        public string ClickUpTaskId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("due_date")]
        public string DueDateSecondsSinceUnixEpochAsString { get; set; }

        [JsonPropertyName("custom_fields")]
        public IEnumerable<ClickUpCustomField> CustomFields { get; set; }

        [JsonPropertyName("list")]
        public ClickUpList List { get; set; }

        [JsonPropertyName("folder")]
        public ClickUpId Folder { get; set; }

        [JsonPropertyName("space")]
        public ClickUpId Space { get; set; }
    }
}
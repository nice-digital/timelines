using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
    public class ClickUpTasks
    {
        [JsonPropertyName("tasks")]
        public IEnumerable<ClickUpTask> Tasks { get; set; }
    }
}
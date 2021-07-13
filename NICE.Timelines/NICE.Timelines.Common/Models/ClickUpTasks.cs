using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Common.Models
{
    public class ClickUpTasks
    {
        [JsonPropertyName("tasks")]
        public IEnumerable<ClickUpTask> Tasks { get; set; }
    }
}
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Common.Models
{
    public class ClickUpTypeConfig
    {
        [JsonPropertyName("options")]
        public IList<ClickUpOption> Options { get; set; }
    }
}
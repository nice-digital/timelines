using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
    public class ClickUpFolders
    {
        [JsonPropertyName("folders")]
        public IList<ClickUpFolder> Folders { get; set; }
    }
}
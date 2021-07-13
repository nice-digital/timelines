using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Common.Models
{
    public class ClickUpFolders
    {
        [JsonPropertyName("folders")]
        public IList<ClickUpFolder> Folders { get; set; }
    }
}
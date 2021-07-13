using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Common.Models
{
    public class ClickUpLists
    {
        [JsonPropertyName("lists")]
        public IList<ClickUpList> Lists { get; set; }
    }
}
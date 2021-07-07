using System;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
    public class ClickUpOption
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
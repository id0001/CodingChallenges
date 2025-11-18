using System.Text.Json.Serialization;

namespace EverybodyCodes.Core.Models
{
    public record Keys
    {
        [JsonPropertyName("key1")]
        public required string Key1 { get; init; }
        
        [JsonPropertyName("key2")]
        public string? Key2 { get; init; }

        [JsonPropertyName("key3")]
        public string? Key3 { get; init; }
    }
}

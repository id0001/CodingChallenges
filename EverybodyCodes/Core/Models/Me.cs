using System.Text.Json.Serialization;

namespace EverybodyCodes.Core.Models
{
    public record Me
    {
        [JsonPropertyName("id")]
        public required int Id { get; init; }

        [JsonPropertyName("seed")]
        public required int Seed { get; init; }
    }
}

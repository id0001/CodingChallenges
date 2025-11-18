namespace EverybodyCodes.Core.Models
{
    public record Config
    {
        public required string SessionToken { get; init; }
        public required string Email { get; init; }
        public required int Seed { get; init; }
    }
}

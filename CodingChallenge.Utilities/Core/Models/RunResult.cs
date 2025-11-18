namespace CodingChallenge.Utilities.Core.Models
{
    public record RunResult(int Challenge, int Part, string Result, string? ExpectedResult, TimeSpan Duration);
}

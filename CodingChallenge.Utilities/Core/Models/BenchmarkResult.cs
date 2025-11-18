namespace CodingChallenge.Utilities.Core.Models
{
    public sealed record BenchmarkResult(int Challenge, int Part, string Result, string? ExpectedResult, TimeSpan Duration, BenchmarkTime? Benchmark);

    public sealed record BenchmarkTime(IReadOnlyCollection<TimeSpan> Durations)
    {
        public int RunCount => Durations.Count;

        public TimeSpan Fastest => Durations.Min();

        public TimeSpan Slowest => Durations.Max();

        public TimeSpan Average => Durations.Aggregate(TimeSpan.Zero, (acc, ts) => acc.Add(ts)) / Durations.Count;
    }
}

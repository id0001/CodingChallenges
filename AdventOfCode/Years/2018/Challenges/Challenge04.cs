using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Challenges;

[Challenge(4)]
public partial class Challenge04
{
    private const string _beginShiftPattern = @"Guard #(\d+) begins shift";

    [Part(1, "36898")]
    public string Part1(string input)
    {
        var logs = ParseLogs(input.Lines(ParseLine).OrderBy(log => log.Time));
        var guard = logs.OrderByDescending(x => x.Value.Sum(a => a.TotalMinutes)).First();
        var minute = ExtractAsleepCountPerMinute(guard.Value).Index().OrderByDescending(x => x.Item).First().Index;
        return (guard.Key * minute).ToString();
    }

    [Part(2, "80711")]
    public string Part2(string input)
    {
        var logs = ParseLogs(input.Lines(ParseLine).OrderBy(log => log.Time));
        var result = logs.Select(kv =>
        {
            var asleep = ExtractAsleepCountPerMinute(kv.Value).Index().OrderByDescending(x => x.Item).First();
            return (Id: kv.Key, asleep.Index, asleep.Item);
        })
        .OrderByDescending(x => x.Item)
        .First();

        return (result.Id * result.Index).ToString();
    }

    private static int[] ExtractAsleepCountPerMinute(IEnumerable<Asleep> list) => Enumerable
        .Range(0, 60)
        .Select(minute => list.Count(asleep => asleep.From.Minute <= minute && asleep.To.Minute > minute))
        .ToArray();

    private static (DateTime Time, string Action) ParseLine(string line) => line.Extract<DateTime, string>(@"\[(.+)] (.+)");

    private static Dictionary<int, List<Asleep>> ParseLogs(IEnumerable<(DateTime Time, string Action)> logs)
    {
        var dict = new Dictionary<int, List<Asleep>>();
        foreach (var chunk in logs.ChunkBy(log => BeginShiftPattern().IsMatch(log.Action)).Select(c => c.ToArray()))
        {
            var id = chunk.First().Action.Extract<int>(_beginShiftPattern);
            var asleep = chunk.Skip(1).Chunk(2).Select(parts => new Asleep(parts.First().Time, parts.Second().Time)).ToList();
            if (!dict.TryAdd(id, asleep))
                dict[id].AddRange(asleep);
        }

        return dict;
    }

    private record Asleep(DateTime From, DateTime To)
    {
        public int TotalMinutes { get; } = (To - From).Minutes;
    }

    [GeneratedRegex(_beginShiftPattern)]
    private static partial Regex BeginShiftPattern();
}

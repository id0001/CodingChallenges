using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Collections.Immutable;

namespace AdventOfCode2016.Challenges;

[Challenge(4)]
public class Challenge04
{
    [Part(1, "361724")]
    public string Part1(string input) => input.Lines(ParseLine).Where(msg => msg.IsValid).Sum(msg => msg.SectorId).ToString();

    [Part(2, "482")]
    public string Part2(string input) => input
        .Lines(ParseLine)
        .Where(msg => msg.IsValid && msg.Decrypted.StartsWith("northpole", StringComparison.OrdinalIgnoreCase))
        .Select(x => x.SectorId)
        .First()
        .ToString();

    private static Message ParseLine(string line) => line.SplitBy("-").Transform(x =>
    {
        var words = x[..^1].ToImmutableArray();
        var (sectorId, checksum) = x[^1].Extract(@"(\d+)\[([a-z]+)\]").Transform(arr => (arr.First().As<int>(), arr.Second().ToImmutableHashSet()));
        return new Message(words, sectorId, checksum);
    });

    private record Message(ImmutableArray<string> Words, int SectorId, ImmutableHashSet<char> Checksum)
    {
        public bool IsValid { get; init; } = Words
            .SelectMany(_ => _)
            .GetCharCount()
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .Take(5)
            .All(Checksum.Contains);

        public string Decrypted { get; init; } = string.Join(' ', Words.Select(w => w.CaesarShift(SectorId)));
    }
}

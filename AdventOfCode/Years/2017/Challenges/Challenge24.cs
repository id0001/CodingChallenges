using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Collections.Immutable;

namespace AdventOfCode2017.Challenges;

[Challenge(24)]
public class Challenge24
{
    [Part(1, "1906")]
    public string Part1(string input)
    {
        var bridges = input.Lines(ParseLine).ToList();

        return EnumerateBridges(bridges, 0, [])
            .Max(arr => arr.Sum(b => b.Strength))
            .ToString();
    }

    [Part(2, "1824")]
    public string Part2(string input)
    {
        var bridges = input.Lines(ParseLine).ToList();

        return EnumerateBridges(bridges, 0, [])
            .OrderByDescending(arr => arr.Count)
            .Select(arr => arr.Sum(b => b.Strength))
            .First()
            .ToString();
    }

    private static IEnumerable<ImmutableHashSet<Bridge>> EnumerateBridges(IEnumerable<Bridge> bridges, int pins, ImmutableHashSet<Bridge> current)
    {
        var available = bridges.Where(b => b.IsOption(pins) && !current.Contains(b)).ToList();
        if (available.Count == 0)
        {
            yield return current;
            yield break;
        }

        foreach (var option in available)
        {
            foreach (var item in EnumerateBridges(bridges, option.Other(pins), current.Add(option)))
                yield return item;
        }
    }

    private static Bridge ParseLine(string line)
        => line.Extract<int, int>(@"(\d+)/(\d+)").Transform(parts => new Bridge(parts.First, parts.Second));

    private record Bridge(int A, int B)
    {
        public int Strength = A + B;

        public bool IsOption(int pins) => A == pins || B == pins;

        public int Other(int pins) => A == pins ? B : A;
    }
}

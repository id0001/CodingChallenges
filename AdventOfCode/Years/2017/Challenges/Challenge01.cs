using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(1)]
public class Challenge01
{
    [Part(1, "1171")]
    public string Part1(string input) => input
        .Select(c => c.AsInteger)
        .Transform(list => list
            .Cycle()
            .Skip(1)
            .Zip(list)
            .Where(pair => pair.First == pair.Second)
            .Select(pair => pair.First)
            .Sum()
        ).ToString();

    [Part(2, "1024")]
    public string Part2(string input) => input
        .Select(c => c.AsInteger)
        .ToList()
        .Transform(list => list
            .Cycle()
            .Skip(list.Count / 2)
            .Zip(list)
            .Where(pair => pair.First == pair.Second)
            .Select(pair => pair.First)
            .Sum()
        ).ToString();
}

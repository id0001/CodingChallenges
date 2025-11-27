using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(2)]
public class Challenge02
{
    [Part(1, "36766")]
    public string Part1(string input) => input
        .Lines(line => line
            .SplitBy<int>("\t")
            .Transform(row => row.Max() - row.Min()))
        .Sum()
        .ToString();

    [Part(2, "261")]
    public string Part2(string input) => input
        .Lines(line => line
            .SplitBy<int>("\t")
            .Combinations(2)
            .Where(pair => Math.Max(pair[0], pair[1]).Mod(Math.Min(pair[0], pair[1])) == 0)
            .Select(pair => Math.Max(pair[0], pair[1]) / Math.Min(pair[0], pair[1]))
            .Single())
        .Sum()
        .ToString();
}

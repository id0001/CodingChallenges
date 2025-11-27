using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(4)]
public class Challenge04
{
    [Part(1, "466")]
    public string Part1(string input) => input
        .Lines()
        .Where(l => l
            .SplitBy(" ")
            .Transform(p => p.Length == p
                .Distinct()
                .Count()))
        .Count()
        .ToString();

    [Part(2, "251")]
    public string Part2(string input) => input
        .Lines()
        .Where(l => !l
            .SplitBy(" ")
            .Combinations(2)
            .Any(p => p.First().IsAnagramOf(p.Second())))
        .Count()
        .ToString();
}

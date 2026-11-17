using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2020.Challenges;

[Challenge(6)]
public class Challenge06
{
    [Part(1, "6585")]
    public string Part1(string input) => input
        .Paragraphs()
        .Sum(p => p
            .Lines()
            .SelectMany(l => l)
            .Distinct()
            .Count()
        )
        .ToString();

    [Part(2, "3276")]
    public string Part2(string input) => input
        .Paragraphs()
        .Sum(p => p
            .Lines()
            .Aggregate((a, b) => new string([.. a.Intersect(b)]))
            .Length
        )
        .ToString();
}

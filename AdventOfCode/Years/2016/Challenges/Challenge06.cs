using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(6)]
public class Challenge06
{
    [Part(1, "liwvqppc")]
    public string Part1(string input) => input
        .ToGrid()
        .Columns()
        .Select(col => col.GetCharCount().MaxBy(x => x.Value).Key)
        .AsString();

    [Part(2, "caqfbzlh")]
    public string Part2(string input) => input
        .ToGrid()
        .Columns()
        .Select(col => col.GetCharCount().MinBy(x => x.Value).Key)
        .AsString();
}

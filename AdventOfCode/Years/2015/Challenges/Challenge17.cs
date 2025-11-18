using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(17)]
public class Challenge17
{
    [Part(1, "1638")]
    public string Part1(string input)
    {
        var items = input.Lines().As<int>().ToArray();
        return Enumerable
            .Range(1, items.Length)
            .SelectMany(k => items.Combinations(k))
            .Count(c => c.Sum() == 150)
            .ToString();
    }

    [Part(2, "17")]
    public string Part2(string input)
    {
        var items = input.Lines().As<int>().ToArray();

        for (var i = 1; i <= items.Length; i++)
        {
            var combinations = items.Combinations(i).Count(c => c.Sum() == 150);
            if (combinations > 0)
                return combinations.ToString();
        }

        throw new InvalidOperationException();
    }
}

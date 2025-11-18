using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(24)]
public class Challenge24
{
    [Part(1, "10439961859")]
    public string Part1(string input)
    {
        var weights = input.Lines().As<long>().OrderDescending().ToList();
        var targetTotal = weights.Sum() / 3;

        return Enumerable.Range(4, weights.Count)
            .SelectMany(i => weights
                .Combinations(i)
                .Where(x => x.Sum() == targetTotal)
                .Select(x => x.Product())
                .Order())
            .First()
            .ToString();
    }

    [Part(2, "72050269")]
    public string Part2(string input)
    {
        var weights = input.Lines().As<long>().OrderDescending().ToList();
        var targetTotal = weights.Sum() / 4;

        return Enumerable.Range(4, weights.Count)
            .SelectMany(i => weights
                .Combinations(i)
                .Where(x => x.Sum() == targetTotal)
                .Select(x => x.Product())
                .Order())
            .First()
            .ToString();
    }
}

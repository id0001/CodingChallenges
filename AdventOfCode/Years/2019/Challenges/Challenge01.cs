using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(1)]
public class Challenge01
{
    [Part(1, "3394106")]
    public string Part1(string input) => input.Lines(int.Parse).Aggregate(0, (sum, v) => sum + (v / 3 - 2)).ToString();

    [Part(2, "5088280")]
    public string Part2(string input) => input.Lines(int.Parse).Aggregate(0, (sum, v) => sum + CalcFuel(v)).ToString();

    private static int CalcFuel(int mass) => mass > 0 ? Math.Max(0, mass / 3 - 2) + CalcFuel(mass / 3 - 2) : 0;
}

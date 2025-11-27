using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(11)]
public class Challenge11
{
    [Part(1, "773")]
    public string Part1(string input) => Hex
        .ManhattanDistance(Hex.Zero, input
            .SplitBy(",")
            .Aggregate(Hex.Zero, Next))
        .ToString();

    [Part(2, "1560")]
    public string Part2(string input) => input
        .SplitBy(",")
        .Aggregate((MaxDistance: 0, Hex: Hex.Zero), NextWithMaxDistance)
        .MaxDistance
        .ToString();

    private static Hex Next(Hex current, string dir) => dir switch
    {
        "n" => current.North,
        "ne" => current.NorthEast,
        "se" => current.SouthEast,
        "s" => current.South,
        "sw" => current.SouthWest,
        "nw" => current.NorthWest,
        _ => throw new NotImplementedException()
    };

    private static (int MaxDistance, Hex Hex) NextWithMaxDistance((int MaxDistance, Hex Hex) current, string dir)
    {
        var next = Next(current.Hex, dir);
        return (Math.Max(current.MaxDistance, Hex.ManhattanDistance(Hex.Zero, next)), next);
    }
}

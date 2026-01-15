using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(3)]
public class Challenge03
{
    [Part(1, "104241")]
    public string Part1(string input)
    {
        var map = new SpatialMap2<int>();
        foreach(var claim in input.Lines(ParseLine))
        {
            foreach (var point in claim.Square.EnumeratePointsInRectangle())
                map[point]++;
        }

        return map.Select(x => x.Value).Count(x => x > 1).ToString();
    }

    [Part(2, "806")]
    public string Part2(string input)
    {
        var claims = input.Lines(ParseLine).ToList();
        return claims.First(c => !claims.Where(c2 => c != c2).Any(c2 => c.Square.IntersectsWith(c2.Square))).Id;
    }

    private static Claim ParseLine(string line) => line
        .Extract<string, int, int, int, int>(@"#(.+) @ (\d+),(\d+): (\d+)x(\d+)")
        .Transform(x => new Claim(x.First, new Rectangle(x.Second, x.Third, x.Fourth, x.Fifth)));

    private record Claim(string Id, Rectangle Square);
}

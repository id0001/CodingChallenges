using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(6)]
public class Challenge06
{
    [Part(1, "400410")]
    public string Part1(string input)
    {
        var grid = new bool[1000, 1000];
        foreach (var (type, from, to) in input.Lines(Extract))
        {
            var rect = new Rectangle(from, to - from);
            foreach (var p in rect.EnumeratePointsInRectangle())
            {
                grid[p.Y, p.X] = type switch
                {
                    "turn off" => false,
                    "turn on" => true,
                    "toggle" => !grid[p.Y, p.X],
                    _ => throw new NotImplementedException()
                };
            }
        }

        return grid.Count(x => x == true).ToString();
    }

    [Part(2, "15343601")]
    public string Part2(string input)
    {
        var grid = new int[1000, 1000];
        foreach (var (type, from, to) in input.Lines(Extract))
        {
            var rect = new Rectangle(from, to - from);
            foreach (var p in rect.EnumeratePointsInRectangle())
            {
                grid[p.Y, p.X] = Math.Max(0, grid[p.Y, p.X] + type switch
                {
                    "turn off" => -1,
                    "turn on" => 1,
                    "toggle" => 2,
                    _ => throw new NotImplementedException()
                });
            }
        }

        return grid.Sum().ToString();
    }

    public static (string, Point2, Point2) Extract(string line) => line
        .Extract<string, int, int, int, int>(@"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)")
        .Transform(t => (t.First, new Point2(t.Second, t.Third), new Point2(t.Fourth + 1, t.Fifth + 1)));
}

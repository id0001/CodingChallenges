using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(6)]
public class Challenge06
{
    [Part(1, "3010")]
    public string Part1(string input)
    {
        var coords = input
            .Lines(line => line
                .SplitBy<int>(",")
                .Transform(p => new Point2(p.First(), p.Second())))
            .ToList();

        var areas = new Dictionary<Point2, int>();
        var cloud = new PointCloud2(coords);

        foreach (var p in cloud.Bounds.EnumeratePointsInRectangle())
        {
            if (!TryGetClosestCoord(p, coords, out var closest))
                continue;

            areas.TryAdd(closest, 0);
            if (IsOnInfiniteBorder(p, cloud.Bounds))
                areas[closest] = int.MaxValue;
            else if (areas[closest] != int.MaxValue)
                areas[closest]++;
        }

        return areas.Where(kv => kv.Value != int.MaxValue).Max(kv => kv.Value).ToString();
    }

    [Part(2, "48034")]
    public string Part2(string input)
    {
        var coords = input
            .Lines(line => line
                .SplitBy<int>(",")
                .Transform(p => new Point2(p.First(), p.Second())))
            .ToList();

        var cloud = new PointCloud2(coords);

        var totalArea = 0;
        foreach (var p in cloud.Bounds.EnumeratePointsInRectangle())
        {
            if (coords.Sum(c => Point2.ManhattanDistance(c, p)) < 10000)
                totalArea++;
        }

        return totalArea.ToString();
    }

    private static bool TryGetClosestCoord(Point2 value, IReadOnlyCollection<Point2> coords, out Point2 result)
    {
        result = Point2.Zero;

        var values = coords
            .OrderBy(p => Point2.ManhattanDistance(p, value))
            .Take(2)
            .ToArray();

        if (Point2.ManhattanDistance(values[0], value) == Point2.ManhattanDistance(values[1], value))
            return false;

        result = values[0];
        return true;
    }

    private static bool IsOnInfiniteBorder(Point2 value, Rectangle bounds)
        => value.X == bounds.Left || value.Y == bounds.Top || value.X == bounds.Right - 1 || value.Y == bounds.Bottom - 1;
}
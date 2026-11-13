using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(17)]
public class Challenge17
{
    [Part(1, "50838")]
    public string Part1(string input)
    {
        var sand = new PointCloud2(input.Lines(ParseLine).SelectMany(x => x));
        var water = new SpatialMap2<Water>();

        Flow(new Point2(500, 0), sand, water);

        return water
            .Count(x => x.Key.Y >= sand.Bounds.Top && x.Key.Y <= sand.Bounds.Bottom)
            .ToString();
    }

    [Part(2, "43039")]
    public string Part2(string input)
    {
        var sand = new PointCloud2(input.Lines(ParseLine).SelectMany(x => x));
        var water = new SpatialMap2<Water>();

        Flow(new Point2(500, 0), sand, water);

        return water
            .Count(x => x.Key.Y >= sand.Bounds.Top && x.Key.Y <= sand.Bounds.Bottom && x.Value == Water.Settled)
            .ToString();
    }

    private static bool Flow(Point2 current, PointCloud2 sand, SpatialMap2<Water> water)
    {
        if (current.Y >= sand.Bounds.Bottom)
            return true; // Flowing out of bounds

        water[current] = Water.Flowing;

        var hitVoid = false;
        if (CanFlowDown(current, sand, water))
            hitVoid = Flow(current.Down, sand, water);

        if(CanFlowSideways(current, Face.Left, sand,water))
            hitVoid = Flow(current.Left, sand, water);

        if (CanFlowSideways(current, Face.Right, sand, water))
            hitVoid = Flow(current.Right, sand, water);

        if (!hitVoid && water[current] == Water.Flowing)
            Settle(current, sand, water);

        return hitVoid;
    }

    private static void Settle(Point2 current, PointCloud2 sand, SpatialMap2<Water> water)
    {
        var toSettle = new List<Point2>();
        for (var i = current.X; ; i--)
        {
            var p = new Point2(i, current.Y);
            if(sand.Contains(p))
                break; // hit wall

            if (!sand.Contains(p) && water[p] == Water.None)
                return; // hit empty air. Cannot settle;

            toSettle.Add(p);
        }

        for(var i = current.X; ; i++)
        {
            var p = new Point2(i, current.Y);
            if (sand.Contains(p))
                break; // hit wall

            if(!sand.Contains(p) && water[p] == Water.None)
                return; // hit empty air. Cannot settle;

            toSettle.Add(p);
        }

        foreach(var p in toSettle)
            water[p] = Water.Settled;
    }

    private static bool CanFlowDown(Point2 current, PointCloud2 sand, SpatialMap2<Water> water) =>
        IsEmpty(current.Down, sand, water);

    private static bool CanFlowSideways(Point2 current, Point2 direction, PointCloud2 sand, SpatialMap2<Water> water) =>
        IsEmpty(current + direction, sand, water) && IsSolidOrSettled(current.Down, sand, water);

    private static bool IsSolidOrSettled(Point2 p, PointCloud2 sand, SpatialMap2<Water> water) =>
        sand.Contains(p) || water[p] == Water.Settled;

    private static bool IsEmpty(Point2 p, PointCloud2 sand, SpatialMap2<Water> water) =>
        !sand.Contains(p) && water[p] == Water.None;

    private static IEnumerable<Point2> ParseLine(string line)
        => line.SplitBy(",")
        .Transform(parts =>
        {
            var (c, a) = parts.First().Extract<char, int>(@"(x|y)=(\d+)");
            var (bl, br) = parts.Second().Extract<int, int>(@"(\d+)..(\d+)");
            return ParsePoints(a, Enumerable.Range(bl, br - bl + 1), c == 'y');
        });

    private static IEnumerable<Point2> ParsePoints(int a, IEnumerable<int> range, bool flipped) =>
        range.Select(b => flipped ? new Point2(b, a) : new Point2(a, b));

    private enum Water
    {
        None = 0,
        Flowing = 1,
        Settled = 2
    }
}

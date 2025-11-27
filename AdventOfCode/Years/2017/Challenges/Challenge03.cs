using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(3)]
public class Challenge03
{
    [Part(1, "438")]
    public string Part1(string input)
    {
        var n = input.As<int>();

        var p = SpiralEnumerable().Skip(n - 1).First();
        return Point2.ManhattanDistance(Point2.Zero, p).ToString();
    }

    [Part(2, "266330")]
    public string Part2(string input)
    {
        var n = input.As<int>();

        var map = new SpatialMap2<int>();
        map[Point2.Zero] = 1;

        foreach(var p in SpiralEnumerable().Skip(1))
        {
            map[p] = p.GetNeighbors(true).Sum(n => map.GetValueOrDefault(n, 0));
            if (map[p] > n)
                return map[p].ToString();
        }

        throw new InvalidOperationException();
    }

    private IEnumerable<Point2> SpiralEnumerable()
    {
        Pose2 pose = new Pose2(Point2.Zero, Face.Right);
        int turnAfter = 1;
        int steps = 0;

        yield return pose.Position;
        while (true)
        {
            pose = pose.Step();
            yield return pose.Position;

            steps++;

            switch (steps)
            {
                case var _ when steps == turnAfter * 2:
                    pose = pose.TurnLeft();
                    steps = 0;
                    turnAfter++;
                    break;
                case var _ when steps == turnAfter:
                    pose = pose.TurnLeft();
                    break;
                default:
                    break;
            }
        }
    }
}

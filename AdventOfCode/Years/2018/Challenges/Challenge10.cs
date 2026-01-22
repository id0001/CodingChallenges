using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;
using System.Text;

namespace AdventOfCode2018.Challenges;

[Challenge(10)]
public class Challenge10
{
    [Part(1, "KBJHEZCB")]
    public string Part1(string input)
    {
        var points = input.Lines(ParseLine).ToArray();
        while (true)
        {
            for (var i = 0; i < points.Length; i++)
                points[i] = points[i].Next();

            if(TryGetResult(points.Select(x => x.Position), out var value))
            {
                var ocr = value.Ocr();
                if (ocr.Length > 0)
                    return ocr;
            }
        }
    }

    [Part(2, "10369")]
    public string Part2(string input)
    {
        var points = input.Lines(ParseLine).ToArray();
        for(var s = 1; ; s++)
        {
            for (var i = 0; i < points.Length; i++)
                points[i] = points[i].Next();

            if (TryGetResult(points.Select(x => x.Position), out var value))
            {
                var ocr = value.Ocr();
                if (ocr.Length > 0)
                    return s.ToString();
            }
        }
    }

    private static bool TryGetResult(IEnumerable<Point2> points, out string result)
    {
        result = string.Empty;
        var cloud = new PointCloud2(points);
        if (cloud.Bounds.Width >= 100)
            return false;

        var sb = new StringBuilder();
        for (var y = cloud.Bounds.Top; y < cloud.Bounds.Bottom; y++)
        {
            for (var x = cloud.Bounds.Left; x < cloud.Bounds.Right; x++)
            {
                sb.Append(cloud.Contains(new Point2(x, y)) ? '#' : '.');
            }

            sb.AppendLine();
        }

        result = sb.ToString();
        return true;
    }

    private static PosVel ParseLine(string line) => line
        .Extract<int, int, int, int>(@"position=< ?(-?\d+),  ?(-?\d+)> velocity=< ?(-?\d+),  ?(-?\d+)>")
        .Transform(m => new PosVel(new Point2(m.First, m.Second), new Point2(m.Third, m.Fourth)));

    private record PosVel(Point2 Position, Point2 Velocity)
    {
        public PosVel Next() => new(Position + Velocity, Velocity);
    }
}

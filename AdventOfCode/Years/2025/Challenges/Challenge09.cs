using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2025.Challenges;

[Challenge(9)]
public class Challenge09
{
    [Part(1, "4771508457")]
    public string Part1(string input)
    {
        var points = input
            .Lines(line => line.SplitBy<int>(",").Transform(p => new Point2(p.First(), p.Second())))
            .ToList();

        var largest = new Rectangle(0, 0, 0, 0);
        for (var i = 0; i < points.Count - 1; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                Point2 tl = points[i];
                Point2 br = points[j];

                if (points[j].X < points[i].X && points[j].Y < points[i].Y)
                    (tl, br) = (br, tl);

                var rec = new Rectangle(tl, (br + Point2.One) - tl);
                if (rec.LongArea > largest.LongArea)
                    largest = rec;
            }
        }

        return largest.LongArea.ToString();
    }

    [Part(2, "1539809693")]
    public string Part2(string input)
    {
        var points = input
            .Lines(line => line.SplitBy<int>(",").Transform(p => new Point2(p.First(), p.Second())))
            .ToList();

        var polygon = new Polygon(points);

        var largest = EnumerateRectangles(points)
            .OrderByDescending(r => r.LongArea)
            .First(r => Contains(r, polygon));

        return largest.LongArea.ToString();
    }

    private static IEnumerable<Rectangle> EnumerateRectangles(List<Point2> points)
    {
        var visited = new HashSet<Rectangle>();
        for (var i = 0; i < points.Count - 1; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                Point2 tl = points[i];
                Point2 br = points[j];

                if (br.X < tl.X)
                    (tl, br) = (tl with { X = br.X }, br with { X = tl.X });

                if (br.Y < tl.Y)
                    (tl, br) = (tl with { Y = br.Y }, br with { Y = tl.Y });

                var rec = new Rectangle(tl, (br + Point2.One) - tl);
                if (visited.Contains(rec))
                    continue;

                visited.Add(rec);

                yield return rec;
            }
        }
    }

    private static bool Contains(Rectangle rect, Polygon polygon)
    {
        var tl = new Point2(rect.Left, rect.Top);
        var tr = new Point2(rect.Right - 1, rect.Top);
        var bl = new Point2(rect.Left, rect.Bottom - 1);
        var br = new Point2(rect.Right - 1, rect.Bottom - 1);

        if (!polygon.Contains(tl) || !polygon.Contains(tr) || !polygon.Contains(bl) || !polygon.Contains(br))
            return false;

        var top = new Segment(tl, tr);
        var right = new Segment(tr, br);
        var bottom = new Segment(br, bl);
        var left = new Segment(bl, tl);

        foreach (var segment in polygon.Segments)
        {
            if (segment.IntersectsWith(top))
                return false;

            if (segment.IntersectsWith(right))
                return false;

            if (segment.IntersectsWith(bottom))
                return false;

            if (segment.IntersectsWith(left))
                return false;
        }

        return true;
    }

    private record Polygon(IList<Point2> Vertices)
    {
        public IReadOnlyList<Segment> Segments { get; } = [.. Vertices.CurrentAndNext(true).Select(s => new Segment(s.Current, s.Next))];

        public bool Contains(Point2 p)
        {
            bool inside = false;
            foreach (var segment in Segments)
            {
                if (IsPointOnSegment(p, segment))
                    return true;

                if((segment.A.Y > p.Y) != (segment.B.Y > p.Y))
                {
                    var xIntersect = ((p.Y - segment.A.Y) * (segment.B.X - segment.A.X)) / (segment.B.Y - segment.A.Y) + segment.A.X;
                    if (p.X < xIntersect)
                        inside = !inside;
                }
            }

            return inside;
        }

        private bool IsPointOnSegment(Point2 point, Segment segment)
        {
            if (segment.A.X <= point.X && point.X <= segment.B.X && segment.A.Y <= point.Y && point.Y <= segment.B.Y)
            {
                var cross = (point.X - segment.A.X) * (segment.B.Y - segment.A.Y) - (point.Y - segment.A.Y) * (segment.B.X - segment.A.X);
                return cross == 0;
            }

            return false;
        }
    }

    private record Segment
    {
        [SetsRequiredMembers]
        public Segment(Point2 a, Point2 b)
        {
            IsHorizontal = a.Y == b.Y;
            A = IsHorizontal ? (a.X < b.X ? a : b) : (a.Y < b.Y ? a : b);
            B = IsHorizontal ? (a.X < b.X ? b : a) : (a.Y < b.Y ? b : a);
        }

        public required Point2 A { get; init; }

        public required Point2 B { get; init; }

        public required bool IsHorizontal { get; init; }

        public bool IntersectsWith(Segment other)
        {
            // NO intersection on parallel lines
            if (IsHorizontal == other.IsHorizontal)
                return false;

            var h = IsHorizontal ? this : other;
            var v = IsHorizontal ? other : this;

            return v.A.X > h.A.X && v.A.X < h.B.X && h.A.Y > v.A.Y && h.A.Y < v.B.Y;
        }

        public bool Contains(Point2 p)
        {
            return A.X <= p.X && p.X <= B.X && A.Y <= p.Y && p.Y <= B.Y;
        }
    }
}

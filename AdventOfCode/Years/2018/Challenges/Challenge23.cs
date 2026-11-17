using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Mathematics;

namespace AdventOfCode2018.Challenges;

[Challenge(23)]
public class Challenge23
{
    [Part(1, "248")]
    public string Part1(string input)
    {
        var nanobots = input.Lines(ParseLine).ToHashSet();

        var strongest = nanobots.MaxBy(n => n.Range)!;
        var inRange = nanobots.Count(n => strongest.IsInRange(n.Position));
        return inRange.ToString();
    }

    [Part(2, "124623002")]
    public string Part2(string input)
    {
        var nanobots = input.Lines(ParseLine).ToHashSet();

        var minX = nanobots.Min(b => b.Position.X);
        var maxX = nanobots.Max(b => b.Position.X);
        var minY = nanobots.Min(b => b.Position.Y);
        var maxY = nanobots.Max(b => b.Position.Y);
        var minZ = nanobots.Min(b => b.Position.Z);
        var maxZ = nanobots.Max(b => b.Position.Z);

        var pq = new PriorityQueue<Cube, Priority>(new PriorityComparer());

        // Make sure the cube is a power of 2 so we can split it evenly
        var cube = ToNearestPowerOf2(new Cube(minX, minY, minZ, maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1));

        pq.Enqueue(cube, new Priority(nanobots.Count, Point3.ManhattanDistance(Point3.Zero, cube.Position), cube.Width));

        while(pq.Count > 0)
        {
            var current = pq.Dequeue();

            if(current.Width == 1)
                return Point3.ManhattanDistance(Point3.Zero, current.Position).ToString();

            foreach(var subCube in current.Subdivide(2))
            {
                var c = nanobots.Count(b => b.Intersects(subCube));
                var d = Point3.ManhattanDistance(Point3.Zero, subCube.Position);
                pq.Enqueue(subCube, new Priority(c, d, subCube.Width));
            }
        }

        throw new InvalidOperationException("No solution found");
    }

    private static Nanobot ParseLine(string line)
        => line
        .Extract<int, int, int, int>(@"pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)")
        .Transform(res => new Nanobot(new Point3(res.First, res.Second, res.Third), res.Fourth));

    private static Cube ToNearestPowerOf2(Cube cube)
    {
        var largest = new[] { cube.Width, cube.Height, cube.Depth }.Max();
        var s = NumberTheory.NextPowerOf2(largest);
        return cube with { Width = s, Height = s, Depth = s };
    }

    private record Nanobot(Point3 Position, int Range)
    {
        public Point3[] Vertices { get; } =
        [
            new (Position.X, Position.Y, Position.Z),
            new (Position.X - Range, Position.Y,Position.Z),
            new (Position.X + Range, Position.Y,Position.Z),
            new (Position.X, Position.Y - Range,Position.Z),
            new (Position.X, Position.Y + Range,Position.Z),
            new (Position.X, Position.Y,Position.Z - Range),
            new (Position.X, Position.Y,Position.Z + Range)
        ];

        public bool IsInRange(Point3 p) => Point3.ManhattanDistance(Position, p) <= Range;

        public bool Intersects(Cube cube)
        {
            // Make sure the coordinate is 1 less than the width/height/depth to be 'in' the cube
            var cubeVerts = new[]
            {
                new Point3(cube.Left, cube.Top, cube.Front),
                new Point3(cube.Right-1, cube.Top, cube.Front),
                new Point3(cube.Right-1, cube.Bottom-1, cube.Front),
                new Point3(cube.Left, cube.Bottom-1, cube.Front),
                new Point3(cube.Left, cube.Top, cube.Back-1),
                new Point3(cube.Right-1, cube.Top, cube.Back-1),
                new Point3(cube.Right-1, cube.Bottom-1, cube.Back-1),
                new Point3(cube.Left ,cube.Bottom-1, cube.Back-1)
            };

            return Vertices.Any(cube.Contains) || cubeVerts.Any(IsInRange);
        }
    }

    private record Priority(int BotsInRange, int DistanceToOrigin, long Size);

    private class PriorityComparer : IComparer<Priority>
    {
        public int Compare(Priority? x, Priority? y)
        {
            if (x is null && y is null)
                return 0;

            if (x is null)
                return -1;

            if (y is null)
                return 1;

            if (x.BotsInRange != y.BotsInRange)
                return y.BotsInRange - x.BotsInRange;

            if (x.DistanceToOrigin != y.DistanceToOrigin)
                return x.DistanceToOrigin - y.DistanceToOrigin;

            return (int)(x.Size - y.Size);
        }
    }
}

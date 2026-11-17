using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(25)]
public class Challenge25
{
    [Part(1, "338")]
    public string Part1(string input)
    {
        var graph = new WeightedGraph<Point4, int>();
        foreach (var point in input.Lines(ParseLine))
            graph.AddVertex(point);

        foreach (var combi in graph.Vertices.Combinations(2))
        {
            var distance = Point4.ManhattanDistance(combi[0], combi[1]);
            if (distance <= 3)
                graph.AddEdge(combi[0], combi[1], distance);
        }

        return graph.ConnectedComponents().Count().ToString();
    }

    private static Point4 ParseLine(string line) => line
        .SplitBy<int>(",")
        .Transform(coords => new Point4(coords.First(), coords.Second(), coords.Third(), coords.Fourth()));

    private record Point4(int X, int Y, int Z, int W)
    {
        public static int ManhattanDistance(Point4 a, Point4 b) =>
            Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z) + Math.Abs(a.W - b.W);
    }
}

using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;
using Spectre.Console;

namespace AdventOfCode2016.Challenges;

[Challenge(22)]
public class Challenge22
{
    [Part(1, "1003")]
    public string Part1(string input)
    {
        var nodes = input.Lines().Skip(2).Select(ParseLine).ToList();

        return nodes
            .SelectMany(a => nodes.Select(b => (A: a, B: b)))
            .Where(pair => pair.A.Used > 0 && pair.A != pair.B && pair.B.Available >= pair.A.Used)
            .Count()
            .ToString();
    }

    [Part(2, "192")]
    public string Part2(string input)
    {
        var nodes = input.Lines().Skip(2).Select(ParseLine).ToDictionary(kv => kv.Location);

        var empty = nodes.Values.First(n => n.Used == 0);
        var maxX = nodes.Keys.MaxBy(n => n.X).X;
        var end = nodes[new Point2(maxX - 1, 0)];

        var path = Graph.Implicit<Node>(n => GetAdjacent(nodes, n)).Bfs().ShortestPath(empty, end);

        // 1. First move to the node on the left of the top right corner using the shortest path.
        // 2. Using a circular movement >V<<^ (5 steps), move the data back to the start. This can be calculated as (5 * X) + 1 -- (+1 for the last move)
        return (path.Length - 1 + end.Location.X * 5 + 1).ToString();
    }

    private static IEnumerable<Edge<Node>> GetAdjacent(Dictionary<Point2, Node> nodes, Node current)
    {
        foreach (var neighbor in current.Location.GetNeighbors())
            if (nodes.ContainsKey(neighbor) && current.Size >= nodes[neighbor].Used)
                yield return new Edge<Node>(current, nodes[neighbor]);
    }

    private Node ParseLine(string line) =>
        line.Extract<int, int, int, int>(@"/dev/grid/node-x(\d+)-y(\d+) +(\d+)T +(\d+)T +\d+T +\d+%")
            .Transform(matches => new Node(new Point2(matches.First, matches.Second), matches.Third, matches.Fourth));

    private record Node(Point2 Location, int Size, int Used)
    {
        public int Available = Size - Used;
    }
}

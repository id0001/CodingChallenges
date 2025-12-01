using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;
using System.Collections;

namespace AdventOfCode2016.Challenges;

[Challenge(24)]
public class Challenge24
{
    [Part(1, "448")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();
        var pois = grid.Where(kv => char.IsNumber(kv.Value)).Select(kv => kv.Key).ToArray();
        var graph = CreateGraph(grid, pois);

        var visited = new BitArray(pois.Length);
        visited.Set(0, true);

        var (_, cost) = Graph.Implicit<Node, int>(n => GetAjacent(graph, n)).Dijkstra().ShortestPath(new Node(0, visited), n => n.Visited.HasAllSet());
        return cost.ToString();
    }

    [Part(2, "672")]
    public string Part2(string input)
    {
        var grid = input.ToGrid();
        var pois = grid.Where(kv => char.IsNumber(kv.Value)).Select(kv => kv.Key).ToArray();
        var graph = CreateGraph(grid, pois);

        var visited = new BitArray(pois.Length);

        var (_, cost) = Graph.Implicit<Node, int>(n => GetAjacent(graph, n)).Dijkstra().ShortestPath(new Node(0, visited), n => n.Visited.HasAllSet());
        return cost.ToString();
    }

    private static WeightedDigraph<int, int> CreateGraph(Grid2<char> grid, Point2[] pois)
    {
        var digraph = new WeightedDigraph<int, int>();
        foreach (var poi in pois)
            digraph.AddVertex(grid[poi].As<int>());

        var graph = Graph.Implicit<Point2>(n => GetAdjacentSimple(grid, n));
        var result = new Dictionary<(int, int), int>();

        foreach (var pair in pois.Combinations(2))
        {
            var distance = graph.Bfs().Distance(pair[0], pair[1]);

            var a = (int)char.GetNumericValue(grid[pair[0]]);
            var b = (int)char.GetNumericValue(grid[pair[1]]);
            digraph.AddEdge(a, b, distance);
            digraph.AddEdge(b, a, distance);
        }

        return digraph;
    }

    private static IEnumerable<(Point2, Point2)> GetAdjacentSimple(Grid2<char> grid, Point2 current)
    {
        foreach (var neighbor in current.GetNeighbors())
            if (grid.Bounds.Contains(neighbor) && grid[neighbor] != '#')
                yield return (current, neighbor);
    }

    private static IEnumerable<(Node, Node, int)> GetAjacent(WeightedDigraph<int, int> pointsOfInterest, Node current)
    {
        foreach (var n in pointsOfInterest.OutEdges(current.Id))
        {
            if (current.Visited[n.Target])
                continue;

            var visitedWithZero = new BitArray(current.Visited);
            visitedWithZero.Set(0, true);

            if (n.Target == 0 && !visitedWithZero.HasAllSet())
                continue; // Can only end at 0 when all other nodes have been visited.

            var visited = new BitArray(current.Visited);
            visited.Set(n.Target, true);
            yield return (current, new Node(n.Target, visited), n.Weight);
        }
    }

    private sealed record Node(int Id, BitArray Visited);
}

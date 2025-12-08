using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(7)]
public class Challenge07
{
    [Part(1, "1698")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();

        var start = grid.First(x => x.Value == 'S').Key;
        var digraph = CreateGraph(grid, start);

        return digraph
            .Vertices
            .Where(v => digraph.OutDegrees(v) == 2)
            .Count()
            .ToString();
    }

    [Part(2, "95408386769474")]
    public string Part2(string input)
    {
        var grid = input.ToGrid();

        var start = grid.First(x => x.Value == 'S').Key;
        var digraph = CreateGraph(grid, start);

        var paths = CountPaths(digraph, start, grid.Rows - 1);

        return paths.ToString();
    }

    private static Digraph<Point2> CreateGraph(Grid2<char> grid, Point2 start)
    {
        var edges = new HashSet<(Point2, Point2)> { (start, start.Down) };
        var graph = new Digraph<Point2>();

        for (var y = 0; y < grid.Rows - 2; y++)
        {
            var nextEdges = new HashSet<(Point2, Point2)>();
            foreach (var (p,v) in edges)
            {
                if (grid[v] == '^')
                {
                    graph.AddEdge(p, v);
                    nextEdges.Add((v, v.Left.Down));
                    nextEdges.Add((v, v.Right.Down));
                    continue;
                }

                nextEdges.Add((p,v.Down));
            }

            edges = nextEdges;
        }

        foreach (var (p, v) in edges)
            graph.AddEdge(p, v);

        return graph;
    }

    private static long CountPaths(Digraph<Point2> graph, Point2 start, int endY)
    {
        var dp = graph.Vertices.ToDictionary(kv => kv, kv => 0L);
        dp[start] = 1;

        foreach (var vertex in graph.TopologicalSort())
            foreach (var neighbor in graph.OutEdges(vertex))
                dp[neighbor.Target] += dp[vertex];

        return dp.Where(v => v.Key.Y == endY).Select(v => v.Value).Sum();
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(12)]
public class Challenge12
{
    [Part(1, "175")]
    public string Part1(string input)
    {
        var graph = ParseGraph(input);

        return graph.Bfs().FloodFill(0).Count().ToString();
    }

    [Part(2, "213")]
    public string Part2(string input)
    {
        var graph = ParseGraph(input);
        var remaining = graph.Vertices.ToHashSet();
        var groups = 0;
        while(remaining.Count > 0)
        {
            remaining.ExceptWith(graph.Bfs().FloodFill(remaining.First()).Select(x => x.Vertex));
            groups++;
        }

        return groups.ToString();
    }

    private static Graph<int> ParseGraph(string input)
    {
        var graph = new Graph<int>();
        foreach (var (p, c) in input.Lines(line => line.Extract<int, string>(@"(\d+) <-> (.+)")))
        {
            var children = c.SplitBy<int>(",");
            foreach (var child in children)
                graph.AddEdge(p, child);
        }

        return graph;
    }
}

using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(11)]
public class Challenge11
{
    [Part(1, "603")]
    public string Part1(string input)
    {
        var graph = CreateGraph(input);
        var cache = new Memoized<Digraph<string>, string, long>(CountPaths1);
        return cache.Invoke(graph, "you").ToString();
    }

    [Part(2, "380961604031372")]
    public string Part2(string input)
    {
        var graph = CreateGraph(input);
        var cache = new Memoized<Digraph<string>, string, bool, bool, long>(CountPaths2);
        return cache.Invoke(graph, "svr", false, false).ToString();
    }

    private static Digraph<string> CreateGraph(string input)
    {
        var graph = new Digraph<string>();
        foreach (var nodes in input.Lines(line => line.Replace(":", string.Empty).SplitBy(" ").ToArray()))
        {
            for (var i = 1; i < nodes.Length; i++)
                graph.AddEdge(nodes[0], nodes[i]);
        }

        return graph;
    }

    private static long CountPaths1(Memoized<Digraph<string>, string, long> self, Digraph<string> graph, string vertex)
    {
        if (vertex == "out")
            return 1;

        long pathCount = 0;
        foreach (var (_, next) in graph.OutEdges(vertex))
            pathCount += self.Invoke(graph, next);

        return pathCount;
    }

    private static long CountPaths2(Memoized<Digraph<string>, string, bool, bool, long> self, Digraph<string> graph, string vertex, bool visitedDac, bool visitedFft)
    {
        if (vertex == "out")
            return visitedFft && visitedDac ? 1 : 0;

        long pathCount = 0;
        foreach (var (_, next) in graph.OutEdges(vertex))
            pathCount += self.Invoke(graph, next, visitedDac || vertex == "dac", visitedFft || vertex == "fft");

        return pathCount;
    }
}

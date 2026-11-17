using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2020.Challenges;

[Challenge(7)]
public class Challenge07
{
    [Part(1, "278")]
    public string Part1(string input)
    {
        var graph = CreateGraph(input);

        var count = 0;
        var visited = new HashSet<string>();
        var stack = new Stack<string>(graph.InEdges("shiny gold").Select(e => e.Source));

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (!visited.Add(current))
                continue;

            visited.Add(current);
            count++;

            foreach (var edge in graph.InEdges(current))
                stack.Push(edge.Source);
        }

        return count.ToString();
    }

    [Part(2, "45157")]
    public string Part2(string input) => CountBags(CreateGraph(input), "shiny gold").ToString();

    private static int CountBags(WeightedDigraph<string, int> graph, string vertex) => graph
        .OutEdges(vertex)
        .Sum(edge => edge.Weight + edge.Weight * CountBags(graph, edge.Target));

    private static WeightedDigraph<string, int> CreateGraph(string input)
    {
        var graph = new WeightedDigraph<string, int>();
        foreach (var line in input.Lines())
        {
            var split = line.SplitBy("contain");
            var from = split.First().Extract<string>(@"([a-z]+ [a-z]+) bags");
            graph.AddVertex(from);

            foreach (var contents in split.Second().SplitBy(","))
            {
                if (!contents.TryExtract<int, string>(@"(\d+) ([a-z]+ [a-z]+) bags?", out var amount, out var to))
                    continue;

                graph.AddVertex(to);
                graph.AddEdge(from, to, amount);
            }
        }

        return graph;
    }
}

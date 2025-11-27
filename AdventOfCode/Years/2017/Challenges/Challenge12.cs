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
        var graph = new DiGraph<int, Edge<int>>();
        foreach (var (p, c) in input.Lines(line => line.Extract<int, string>(@"(\d+) <-> (.+)")))
        {
            var children = c.SplitBy<int>(",");
            foreach(var child in children)
            {
                graph.AddEdge(new Edge<int>(p, child));
                graph.AddEdge(new Edge<int>(child, p));
            }
        }

        return graph.Bfs().FloodFill(0).Count().ToString();
    }

    // [Part(2, "213")]
    // public string Part2(string input)
    // {
    // }
}

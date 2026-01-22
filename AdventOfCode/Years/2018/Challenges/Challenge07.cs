using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(7)]
public class Challenge07
{
    [Part(1, "BCEFLDMQTXHZGKIASVJYORPUWN")]
    public string Part1(string input)
    {
        var actions = input
            .Lines(line => line
                .Extract<char, char>(@"Step (\w) must be finished before step (\w) can begin\."))
            .ToList();

        var graph = new Digraph<char>();
        foreach (var (first, second) in actions)
            graph.AddEdge(first, second);

        return graph.LexicographicalSort().AsString();
    }

    [Part(2, "987")]
    public string Part2(string input)
    {
        var actions = input
            .Lines(line => line
                .Extract<char, char>(@"Step (\w) must be finished before step (\w) can begin\."))
            .ToList();

        var graph = new Digraph<char>();
        foreach (var (first, second) in actions)
            graph.AddEdge(first, second);

        var nodesWork = graph.Vertices.ToDictionary(node => node, node => 61 + (node - 'A'));

        var time = 0;
        while(nodesWork.Keys.Count > 0)
        {
            var available = nodesWork
                .Where(node => graph.InDegrees(node.Key) == 0)
                .OrderBy(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            foreach(var (_, node) in Enumerable.Range(0, 5).Zip(available))
            {
                nodesWork[node]--;
                if (nodesWork[node] == 0)
                {
                    nodesWork.Remove(node);
                    graph.RemoveVertex(node);
                }
            }

            time++;
        }

        return time.ToString();
    }
}

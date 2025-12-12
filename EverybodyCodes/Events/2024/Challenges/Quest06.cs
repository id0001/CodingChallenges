using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Trees;
using CodingChallenge.Utilities.Extensions;

namespace EverybodyCodes2024.Challenges;

[Challenge(6)]
public class Quest06
{
    [Part(1, "RRJQRMCNQMSP@")]
    public string Part1(string input)
    {
        var tree = CreateTree(input);

        var bestFruit = tree
            .FindNodes(n => n.Value.Id == "@")
            .GroupBy(x => x.Depth)
            .Where(x => x.Count() == 1)
            .Select(kv => kv.First())
            .First();

        return BuildPath(bestFruit);
    }

    [Part(2, "RBPCLWTQKZ@")]
    public string Part2(string input)
    {
        var tree = CreateTree(input);
        var count = input.Count(c => c == '@');

        var bestFruit = tree
            .FindNodes(n => n.Value.Id == "@")
            .GroupBy(x => x.Depth)
            .Where(x => x.Count() == 1)
            .Select(kv => kv.First())
            .First();

        return BuildPath(bestFruit, true);
    }

    [Part(3)]
    public string Part3(string input)
    {
        var tree = CreateTree(input);
        var count = input.Count(c => c == '@');

        var bestFruit = tree
            .FindNodes(n => n.Value.Id == "@")
            .GroupBy(x => x.Depth)
            .Where(x => x.Count() == 1)
            .Select(kv => kv.First())
            .First();

        return BuildPath(bestFruit, true);
    }

    private static GenericTree<Node> CreateTree(string input)
    {
        var edges = new List<(Node, Node)>();

        var uniqueKey = 0;
        foreach (var (root, branches) in input.Lines(line => line.Replace(":", ",").SplitBy(",")))
        {
            foreach (var branch in branches)
            {
                edges.Add((new Node(root), branch == "@" ? new Node("@", uniqueKey++) : new Node(branch)));
            }
        }

        return new GenericTree<Node>.Builder(edges).Build();
    }

    private static string BuildPath(GenericTree<Node> leaf, bool takeFirstCharOnly = false)
    {
        var stack = new Stack<string>();
        var current = leaf;
        while (current is { })
        {
            stack.Push(takeFirstCharOnly ? current.Value.Id[0].ToString() : current.Value.Id);
            current = current.Parent;
        }

        return string.Join(string.Empty, stack);
    }

    private record Node(string Id, int? UniqueKey = null);
}

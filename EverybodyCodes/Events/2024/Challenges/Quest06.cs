using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Trees;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Extensions.Trees;

namespace EverybodyCodes2024.Challenges;

[Challenge(6)]
public class Quest06
{
    [Part(1, "RRJQRMCNQMSP@")]
    public string Part1(string input)
    {
        var tree = CreateTree(input);

        var bestFruit = tree
            .FindNodes(n => n.Value == "@")
            .GroupBy(n => n.Depth)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .First();

        return string.Join(string.Empty, bestFruit.Ancestors(true).Reverse());
    }

    [Part(2, "RBPCLWTQKZ@")]
    public string Part2(string input)
    {
        var tree = CreateTree(input);

        var bestFruit = tree
            .FindNodes(n => n.Value == "@")
            .GroupBy(n => n.Depth)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .First();

        return string.Join(string.Empty, bestFruit.Ancestors(true).Reverse().Select(v => v[0]));
    }

    [Part(3, "RKBXWHVBZGNF@")]
    public string Part3(string input)
    {
        var tree = CreateTree(input);

        var bestFruit = tree
            .FindNodes(n => n.Value == "@")
            .GroupBy(n => n.Depth)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .First();

        return string.Join(string.Empty, bestFruit.Ancestors(true).Reverse().Select(v => v[0]));
    }

    private static GenericTree<string> CreateTree(string input)
    {
        var relations = new List<(string, string)>();

        foreach (var (root, branches) in input.Lines(line => line.Replace(":", ",").SplitBy(",")))
        {
            if (root is "ANT" or "BUG")
                continue;

            foreach (var branch in branches.Where(b => b is not "ANT" and not "BUG"))
                relations.Add((root, branch));
        }

        return new GenericTree<string>.Builder(relations).Build();
    }

    private record Node(string value);
}
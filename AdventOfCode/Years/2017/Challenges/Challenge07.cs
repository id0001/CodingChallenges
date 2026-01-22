using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Trees;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(7)]
public class Challenge07
{
    [Part(1, "gmcrj")]
    public string Part1(string input)
    {
        var tree = CreateTree(input);
        return tree.Value.Name;
    }

    [Part(2, "391")]
    public string Part2(string input)
    {
        var tree = CreateTree(input);
        var heavyNode = FindHeavyNode(tree);
        var diff = CalcWeight(heavyNode) - CalcWeight(heavyNode.Siblings.First());
        return (heavyNode.Value.Weight - diff).ToString();
    }

    private static GenericTree<Node> FindHeavyNode(GenericTree<Node> root)
    {
        if (root.Children.Count == 0)
            return root;

        var groupByWeight = root.Children.GroupBy(CalcWeight).ToList();
        if (groupByWeight.Count == 1)
            return root;

        var heavy = groupByWeight.Single(g => g.Count() == 1).Single();
        return FindHeavyNode(heavy);
    }

    private static int CalcWeight(GenericTree<Node> node) => node.Value.Weight + node.Children.Sum(CalcWeight);

    private static GenericTree<Node> CreateTree(string input)
    {
        var nodes = new List<(string, string)>();
        var weights = new Dictionary<string, int>();
        foreach (var line in input.Lines())
        {
            var split = line.Extract(@"(.+) \((\d+)\)(?: -> (.+))?").ToArray();

            var parent = split.First();
            var weight = split.Second().As<int>();

            weights.Add(parent, weight);
            if (split.Length == 3)
            {
                var children = split.Third().SplitBy(",");
                foreach (var child in children)
                    nodes.Add((parent, child));
            }
        }

        return GenericTree.From(nodes.Select(x => (new Node(x.Item1, weights[x.Item1]), new Node(x.Item2, weights[x.Item2]))));
    }

    private record Node(string Name, int Weight);
}

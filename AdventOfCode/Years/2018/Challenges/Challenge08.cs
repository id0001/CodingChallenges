using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Trees;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(8)]
public class Challenge08
{
    [Part(1, "46829")]
    public string Part1(string input)
    {
        var values = input.SplitBy<int>(" ").ToArray();
        var relations = new List<(int[], int[])>();

        ConstructRelations(values, 0, relations);
        var tree = GenericTree.From(relations);

        return tree.Sum(x => x.Value.Sum()).ToString();
    }

    [Part(2, "37450")]
    public string Part2(string input)
    {
        var values = input.SplitBy<int>(" ").ToArray();
        var relations = new List<(int[], int[])>();

        ConstructRelations(values, 0, relations);
        var tree = GenericTree.From(relations);

        var stack = new Stack<GenericTree<int[]>>();
        stack.Push(tree);

        var sum = 0;
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (current.Children.Count == 0)
            {
                sum += current.Value.Sum();
                continue;
            }

            foreach (var i in current.Value.Where(v => v - 1 >= 0 && v - 1 < current.Children.Count))
                stack.Push(current.Children[i - 1]);
        }

        return sum.ToString();
    }

    private static (int[], int) ConstructRelations(int[] source, int index, List<(int[] Parent, int[] Child)> relations)
    {
        var childCount = source[index];
        var metadataCount = source[index + 1];

        var childIndex = index + 2;
        var children = new List<int[]>();
        for (var i = 0; i < childCount; i++)
        {
            var (child, length) = ConstructRelations(source, childIndex, relations);
            children.Add(child);
            childIndex += length;
        }

        var metadata = source[childIndex..(childIndex + metadataCount)];
        foreach (var child in children)
            relations.Add((metadata, child));

        return (metadata, (childIndex + metadataCount) - index);
    }
}

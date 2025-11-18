using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Trees;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(6)]
public class Challenge06
{
    [Part(1, "142915")]
    public string Part1(string input)
    {
        var root = new GenericTree<string>.Builder(input.Lines(line => line.SplitBy<string, string>(")"))).Build();
        return SumDepth(root).ToString();
    }

    [Part(2, "283")]
    public string Part2(string input)
    {
        var root = new GenericTree<string>.Builder(input.Lines(line => line.SplitBy<string, string>(")"))).Build();
        root = root.FindNode("YOU");
        root.MakeRoot();
        return (root.FindNode("SAN").Depth - 2).ToString();
    }

    private int SumDepth(GenericTree<string> root)
    {
        return root.Children.Sum(SumDepth) + root.Depth;
    }
}

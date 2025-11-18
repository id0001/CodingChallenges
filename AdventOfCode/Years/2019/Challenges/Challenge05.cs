using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(5)]
public class Challenge05
{
    [Part(1, "7988899")]
    public string Part1(string input)
    {
        var program = input.SplitBy<int>(",").ToArray();
        var cpu = new Cpu<int>(program);
        cpu.RunTillHalted(1);
        return cpu.ReadAll().Last().ToString();
    }

    [Part(2, "13758663")]
    public string Part2(string input)
    {
        var program = input.SplitBy<int>(",").ToArray();
        var cpu = new Cpu<int>(program);
        cpu.RunTillHalted(5);
        return cpu.ReadAll().Last().ToString();
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(9)]
public class Challenge09
{
    [Part(1, "3100786347")]
    public string Part1(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();

        var cpu = new Cpu<long>(program);
        cpu.RunTillHalted(1);
        return cpu.Read().ToString();
    }

    [Part(2, "87023")]
    public string Part2(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();

        var cpu = new Cpu<long>(program);
        cpu.RunTillHalted(2);
        return cpu.Read().ToString();
    }
}

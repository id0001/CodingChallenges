using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(5)]
public class Challenge05
{
    [Part(1, "364539")]
    public string Part1(string input)
    {
        var list = input.Lines(x => x.As<int>()).ToList();

        var ip = 0;
        var steps = 0;
        while(ip < list.Count)
        {
            ip += list[ip]++;
            steps++;
        }

        return steps.ToString();
    }

    [Part(2, "27477714")]
    public string Part2(string input)
    {
        var list = input.Lines(x => x.As<int>()).ToList();

        var ip = 0;
        var steps = 0;
        while (ip < list.Count)
        {
            var v = ip;
            ip += list[ip];
            list[v] += list[v] >= 3 ? -1 : 1;
            steps++;
        }

        return steps.ToString();
    }
}

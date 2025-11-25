using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(19)]
public class Challenge19
{
    [Part(1, "121")]
    public string Part1(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();
        var cpu = new Cpu<long>(program);

        long beamCount = 0;
        for (var y = 0; y < 50; y++)
        {
            for (var x = 0; x < 50; x++)
            {
                cpu.Reset();
                cpu.RunTillHalted(x, y);
                beamCount += cpu.ReadAll().Sum();
            }
        }

        return beamCount.ToString();
    }

    [Part(2, "15090773")]
    public string Part2(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();
        var cpu = new Cpu<long>(program);

        int x = 0;
        int y = 0;

        while (true)
        {
            while (RunCpu(cpu, x, y + 99) != 1)
                x++;

            if (RunCpu(cpu, x + 99, y) == 1)
                return (x * 10000 + y).ToString();

            y++;
        }
    }

    private int RunCpu(Cpu<long> cpu, params long[] input)
    {
        cpu.Reset();
        cpu.RunTillHalted(input);
        return (int)cpu.ReadAll().Last();
    }
}

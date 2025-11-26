using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(21)]
public class Challenge21
{
    [Part(1, "19357390")]
    public string Part1(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();
        string[] instructions = [
            "NOT C J\n",
            "AND D J\n",
            "NOT B T\n",
            "AND C T\n",
            "OR T J\n",
            "NOT A T\n",
            "AND B T\n",
            "OR T J\n",
            "WALK\n"
            ];

        int ip = 0;
        var cpu = new Cpu<long>(program);
        while (cpu.Next())
        {
            if (cpu.InputNeeded)
                WriteInstructions(cpu, instructions[ip++]);
        }

        return cpu.ReadAll().Last().ToString();
    }

    [Part(2, "1142844041")]
    public string Part2(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();
        string[] instructions = [
           "NOT C J\n",
            "AND D J\n",
            "AND H J\n",
            "NOT B T\n",
            "AND C T\n",
            "AND D T\n",
            "OR T J\n",
            "NOT A T\n",
            "AND B T\n",
            "OR T J\n",
            "NOT A T\n",
            "OR T J\n",
            "RUN\n"
           ];

        int ip = 0;
        var cpu = new Cpu<long>(program);
        while (cpu.Next())
        {
            if (cpu.InputNeeded)
                WriteInstructions(cpu, instructions[ip++]);
        }

        return cpu.ReadAll().Last().ToString();
    }

    private void WriteInstructions(Cpu<long> cpu, string instruction) => cpu.Write([.. instruction]);
}

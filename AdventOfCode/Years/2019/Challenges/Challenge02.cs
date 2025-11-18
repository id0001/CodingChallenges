using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(2)]
public class Challenge02
{
    [Part(1, "4138658")]
    public string Part1(string input)
    {
        var program = input.SplitBy<int>(",").ToArray();
        program[1] = 12;
        program[2] = 2;

        return new Cpu<int>(program).RunTillHalted().ToString();
    }

    [Part(2, "7264")]
    public string Part2(string input)
    {
        var program = input.SplitBy<int>(",").ToArray();

        for (var noun = 0; noun < 100; noun++)
        {
            for (var verb = 0; verb < 100; verb++)
            {
                program[1] = noun;
                program[2] = verb;
                var cpu = new Cpu<int>(program);
                if (cpu.RunTillHalted() == 19690720)
                    return (100 * noun + verb).ToString();
            }
        }

        throw new InvalidOperationException();
    }
}

using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(23)]
public class Challenge23
{
    [Part(1, "307")]
    public string Part1(string input)
    {
        var program = input.Lines(ParseLine).ToArray();
        var memory = new Dictionary<string, int>()
        {
            ["a"] = 0,
            ["b"] = 0
        };

        RunProgram(memory, program);
        return memory["b"].ToString();
    }

    [Part(2, "160")]
    public string Part2(string input)
    {
        var program = input.Lines(ParseLine).ToArray();
        var memory = new Dictionary<string, int>()
        {
            ["a"] = 1,
            ["b"] = 0
        };

        RunProgram(memory, program);
        return memory["b"].ToString();
    }

    private static void RunProgram(Dictionary<string, int> memory, Instruction[] program)
    {
        for (var ip = 0; ip < program.Length && ip >= 0;)
        {
            switch (program[ip])
            {
                case ("hlf", Arguments<string> args):
                    memory[args.A] /= 2;
                    ip++;
                    break;
                case ("tpl", Arguments<string> args):
                    memory[args.A] *= 3;
                    ip++;
                    break;
                case ("inc", Arguments<string> args):
                    memory[args.A]++;
                    ip++;
                    break;
                case ("jmp", Arguments<int> args):
                    ip += args.A;
                    break;
                case ("jie", Arguments<string, int> args):
                    ip += memory[args.A] % 2 == 0 ? args.B : 1;
                    break;
                case ("jio", Arguments<string, int> args):
                    ip += memory[args.A] == 1 ? args.B : 1;
                    break;
                default:
                    throw new NotImplementedException();
            }

        }
    }

    private static Instruction ParseLine(string line) => line
       .SplitBy(" ", ",")
       .Transform(parts => parts.Length switch
       {
           2 when parts[1].Length == 1 => new Instruction(parts.First(), Arguments.Parse<string>(parts[1..])),
           2 => new Instruction(parts.First(), Arguments.Parse<int>(parts.Second().Replace("+", string.Empty))),
           _ => new Instruction(parts.First(), Arguments.Parse<string, int>(parts.Second(), parts.Third().Replace("+", string.Empty)))
       });
}
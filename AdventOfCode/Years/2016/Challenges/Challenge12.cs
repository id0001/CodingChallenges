using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(12)]
public class Challenge12
{
    [Part(1, "318003")]
    public string Part1(string input)
    {
        var program = input.Lines(ParseLine).ToArray();
        var memory = new Dictionary<string, int>()
        {
            {"a", 0},
            {"b", 0},
            {"c", 0},
            {"d", 0}
        };

        RunProgram(memory, program);
        return memory["a"].ToString();
    }

    [Part(2, "9227657")]
    public string Part2(string input)
    {
        var program = input.Lines(ParseLine).ToArray();
        var memory = new Dictionary<string, int>()
        {
            {"a", 0},
            {"b", 0},
            {"c", 1},
            {"d", 0}
        };

        RunProgram(memory, program);
        return memory["a"].ToString();
    }

    private static Instruction ParseLine(string line)
    {
        var args = line.SplitBy(" ");
        if (args.Length == 2)
            return new Instruction(args[0], Arguments.Parse<int>(args[1..]));

        return new Instruction(args[0], Arguments.Parse<int, int>(args[1..]));
    }

    private static void RunProgram(Dictionary<string,int> memory, Instruction[] program)
    {
        for(var ip = 0; ip >= 0 && ip < program.Length;)
        {
            switch (program[ip])
            {
                case ("cpy", Arguments<int, string> args):
                    memory[args.B] = args.A;
                    ip++;
                    break;
                case ("cpy", Arguments<string, string> args):
                    memory[args.B] = memory[args.A];
                    ip++;
                    break;
                case ("jnz", Arguments<int, int> args):
                    ip += (args.A != 0 ? args.B : 1);
                    break;
                case ("jnz", Arguments<string, int> args):
                    ip+= (memory[args.A] != 0 ? args.B : 1);
                    break;
                case ("inc", Arguments<string> args):
                    memory[args.A]++;
                    ip++;
                    break;
                case ("dec", Arguments<string> args):
                    memory[args.A]--;
                    ip++;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

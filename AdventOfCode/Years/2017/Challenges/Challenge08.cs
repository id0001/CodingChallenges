using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(8)]
public class Challenge08
{
    [Part(1, "4888")]
    public string Part1(string input)
    {
        var program = input.Lines(ParseLine).ToArray();
        var registers = new Dictionary<string, int>();
        int maxValue = 0;
        RunProgram(program, registers, ref maxValue);
        return registers.Values.Max().ToString();
    }

    [Part(2, "7774")]
    public string Part2(string input)
    {
        var program = input.Lines(ParseLine).ToArray();
        var registers = new Dictionary<string, int>();
        int maxValue = 0;
        RunProgram(program, registers, ref maxValue);
        return maxValue.ToString();
    }

    private static void RunProgram(Instruction[] program, Dictionary<string, int> registers, ref int maxValue)
    {
        for (var ip = 0; ip < program.Length && ip >= 0;)
        {
            var register = program[ip].Args.Register;
            var updateVal = program[ip] switch
            {
                ("inc", ">", Arguments args) when registers.GetValueOrDefault(args.A, 0) > args.B => args.Mod,
                ("inc", "<", Arguments args) when registers.GetValueOrDefault(args.A, 0) < args.B => args.Mod,
                ("inc", ">=", Arguments args) when registers.GetValueOrDefault(args.A, 0) >= args.B => args.Mod,
                ("inc", "<=", Arguments args) when registers.GetValueOrDefault(args.A, 0) <= args.B => args.Mod,
                ("inc", "==", Arguments args) when registers.GetValueOrDefault(args.A, 0) == args.B => args.Mod,
                ("inc", "!=", Arguments args) when registers.GetValueOrDefault(args.A, 0) != args.B => args.Mod,
                ("dec", ">", Arguments args) when registers.GetValueOrDefault(args.A, 0) > args.B => -args.Mod,
                ("dec", "<", Arguments args) when registers.GetValueOrDefault(args.A, 0) < args.B => -args.Mod,
                ("dec", ">=", Arguments args) when registers.GetValueOrDefault(args.A, 0) >= args.B => -args.Mod,
                ("dec", "<=", Arguments args) when registers.GetValueOrDefault(args.A, 0) <= args.B => -args.Mod,
                ("dec", "==", Arguments args) when registers.GetValueOrDefault(args.A, 0) == args.B => -args.Mod,
                ("dec", "!=", Arguments args) when registers.GetValueOrDefault(args.A, 0) != args.B => -args.Mod,
                _ => 0
            };

            registers[register] = registers.GetValueOrDefault(register, 0) + updateVal;
            ip++;
            maxValue = Math.Max(maxValue, registers.Values.Max());
        }
    }

    private static Instruction ParseLine(string line)
    {
        var (reg, op, mod, a, compare, b) = line.Extract<string, string, int, string, string, int>(@"(\w+) (inc|dec) (-?\d+) if (\w+) ([<>=!]+) (-?\d+)");
        return new Instruction(op, compare, new Arguments(reg, mod, a, b));
    }

    private record Instruction(string Op, string Compare, Arguments Args);

    private record Arguments(string Register, int Mod, string A, int B);
}

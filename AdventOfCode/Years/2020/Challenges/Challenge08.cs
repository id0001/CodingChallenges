using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2020.Challenges;

[Challenge(8)]
public class Challenge08
{
    [Part(1, "1501")]
    public string Part1(string input)
    {
        var program = input.Lines(ParseLine).ToArray();

        var ipHistory = new HashSet<int>();
        var ip = 0;
        var acc = 0;
        while (ip >= 0 && ip < program.Length)
        {
            if (ipHistory.Contains(ip))
                return acc.ToString();

            ipHistory.Add(ip);

            switch (program[ip])
            {
                case ("acc", Arguments<int> args):
                    acc += args.A;
                    ip++;
                    break;
                case ("jmp", Arguments<int> args):
                    ip += args.A;
                    break;
                default:
                    ip++;
                    break;
            }
        }

        throw new InvalidOperationException("No solution found");
    }

    [Part(2, "509")]
    public string Part2(string input)
    {
        var program = input.Lines(ParseLine).ToArray();

        return program
            .Index()
            .Where(x => x.Item.OpCode == "jmp" || x.Item.OpCode == "nop")
            .Select(x => RunProgram(program, x.Index))
            .First(res => res.HasValue)!
            .Value
            .ToString();
    }

    private static int? RunProgram(Instruction[] program, int flipIp)
    {
        var ipHistory = new HashSet<int>();
        var ip = 0;
        var acc = 0;
        bool flipped = false;
        while (ip >= 0 && ip < program.Length)
        {
            if (ipHistory.Contains(ip))
                return null;

            ipHistory.Add(ip);

            var instruction = program[ip];
            if (ip == flipIp && ! flipped)
            {
                flipped = true;
                instruction = instruction with { OpCode = program[ip].OpCode == "jmp" ? "nop" : "jmp" };
            }

            switch (instruction)
            {
                case ("acc", Arguments<int> args):
                    acc += args.A;
                    ip++;
                    break;
                case ("jmp", Arguments<int> args):
                    ip += args.A;
                    break;
                default:
                    ip++;
                    break;
            }
        }

        return acc;
    }

    private static Instruction ParseLine(string line) => line
        .SplitBy(" ")
        .Transform(res => new Instruction(res[0], Arguments.Parse<int>(res[1])));
}

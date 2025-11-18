using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(23)]
public class Challenge23
{
    [Part(1, "11662")]
    public string Part1(string input)
    {
        var program = input.Lines(ParseLine).ToArray();

        var memory = new Dictionary<string, int>
        {
            ["a"] = 7,
            ["b"] = 0,
            ["c"] = 0,
            ["d"] = 0
        };

        RunProgram(memory, program);
        return memory["a"].ToString();
    }

    [Part(2, "479008222")]
    public string Part2(string input)
    {
        var program = input.Lines(ParseLine).ToArray();

        var memory = new Dictionary<string, int>
        {
            ["a"] = 12,
            ["b"] = 0,
            ["c"] = 0,
            ["d"] = 0
        };

        RunProgram(memory, program);
        return memory["a"].ToString();
    }

    private static void RunProgram(Dictionary<string, int> memory, Instruction[] program)
    {
        for (var ip = 0; ip >= 0 && ip < program.Length;)
        {
            // Multiply
            if (ip + 5 < program.Length && program[ip + 5].OpCode == "jnz" && program[ip+5].Arguments.GetValue(1, r => memory[r]) == -5 && program[ip].Arguments[0] is string)
            {
                Multiply(program, memory, ip);
                ip += 6;
                continue;
            }

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
                case ("jnz", var args) when args.Length == 2:
                    var a = args.GetValue(0, r => memory[r]);
                    var b = args.GetValue(1, r => memory[r]);
                    ip += a != 0 ? b : 1;
                    break;
                case ("inc", Arguments<string> args):
                    memory[args.A]++;
                    ip++;
                    break;
                case ("dec", Arguments<string> args):
                    memory[args.A]--;
                    ip++;
                    break;
                case ("tgl", Arguments<string> args):
                    Toggle(program, ip, memory[args.A]);
                    ip++;
                    break;
                case ("tgl", Arguments<int> args):
                    Toggle(program, ip, args.A);
                    ip++;
                    break;
                default:
                    ip++; // Ignore invalid instruction
                    break;
            }
        }
    }

    private Instruction ParseLine(string line)
    {
        var args = line.SplitBy(" ");

        if (args.Length == 2)
            return new Instruction(args[0], Arguments.Parse<int>(args[1..]));

        return new Instruction(args[0], Arguments.Parse<int, int>(args[1..]));
    }

    private static void Toggle(Instruction[] program, int ip, int x)
    {
        var loc = ip + x;

        if (loc < 0 || loc >= program.Length)
            return;

        program[loc] = program[loc] switch
        {
            ("inc", var args) when args.Length == 1 => program[loc] with { OpCode = "dec" },
            (_, var args) when args.Length == 1 => program[loc] with { OpCode = "inc" },
            ("jnz", var args) when args.Length == 2 => program[loc] with { OpCode = "cpy" },
            (_, var args) when args.Length == 2 => program[loc] with { OpCode = "jnz" },
            _ => program[loc]
        };
    }

    private static void Multiply(Instruction[] program, Dictionary<string, int> registers, int ip)
    {
        /*
            cpy b c // (b == value)
            inc a // a == result (not b or d or c)
            dec c
            jnz c -2
            dec d
            jnz d -5 -- d == multiplier (b * d)
        */

        var resultReg = (string)program[ip+1].Arguments[0]; // a
        var v1Reg = (string)program[ip].Arguments[0]; // b (b is not copied to c)
        var v2Reg = (string)program[ip + 4].Arguments[0]; // d

        registers[resultReg] = registers[v1Reg] * registers[v2Reg]; // a = b * d
        registers[v2Reg] = 0; // clear d
    }
}

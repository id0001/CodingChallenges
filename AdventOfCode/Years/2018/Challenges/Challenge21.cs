using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(21)]
public class Challenge21
{
    private delegate void Operation(Arguments<int, int, int> args, int[] registers);

    [Part(1, "12420065")]
    public string Part1(string input)
    {
        var (ipRegister, program) = Parse(input);

        var registers = new int[6];
        Run(ipRegister, program, registers);

        return registers[4].ToString();
    }

    [Part(2, "1670686")]
    public string Part2(string input)
    {
        var seen = new HashSet<int>();
        var lastSeen = 0;

        var r4 = 123;
        while (true)
        {
            r4 &= 456;
            if (r4 == 72)
                break;
        }

        r4 = 0;
        while (true)
        {
            var r5 = r4 | 65536;
            r4 = 10704114;

            while (true)
            {
                var r2 = r5 & 255;
                r4 += r2;
                r4 &= 16777215;
                r4 *= 65899;
                r4 &= 16777215;

                if (256 > r5)
                {
                    if (seen.Contains(r4))
                        return lastSeen.ToString();

                    seen.Add(r4);
                    lastSeen = r4;
                    break;
                }

                r2 = 0;
                while (true)
                {
                    var r3 = r2 + 1;
                    r3 *= 256;
                    if (r3 > r5)
                    {
                        r5 = r2;
                        break;
                    }

                    r2++;
                }
            }
        }
    }

    private static void Run(int ipRegister, Instruction[] program, int[] registers)
    {
        while (registers[ipRegister] >= 0 && registers[ipRegister] < program.Length)
        {
            registers[ipRegister] = program[registers[ipRegister]] switch
            {
                ("addr", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Addr, args, registers),
                ("addi", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Addi, args, registers),
                ("mulr", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Mulr, args, registers),
                ("muli", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Muli, args, registers),
                ("banr", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Banr, args, registers),
                ("bani", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Bani, args, registers),
                ("borr", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Borr, args, registers),
                ("bori", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Bori, args, registers),
                ("setr", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Setr, args, registers),
                ("seti", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Seti, args, registers),
                ("gtir", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Gtir, args, registers),
                ("gtri", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Gtri, args, registers),
                ("gtrr", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Gtrr, args, registers),
                ("eqir", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Eqir, args, registers),
                ("eqri", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Eqri, args, registers),
                ("eqrr", Arguments<int, int, int> args) => ExecuteAndMove(ipRegister, Eqrr, args, registers),
                _ => throw new NotImplementedException(),
            };
        }
    }

    private static int ExecuteAndMove(int ipRegister, Operation op, Arguments<int, int, int> args, int[] registers)
    {
        if (registers[ipRegister] == 28)
            return -1;

        op(args, registers);
        return registers[ipRegister] + 1;
    }

    private static void Addr(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] + registers[args.B];

    private static void Addi(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] + args.B;

    private static void Mulr(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] * registers[args.B];

    private static void Muli(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] * args.B;

    private static void Banr(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] & registers[args.B];

    private static void Bani(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] & args.B;

    private static void Borr(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] | registers[args.B];

    private static void Bori(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] | args.B;

    private static void Setr(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A];

    private static void Seti(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = args.A;

    private static void Gtir(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = args.A > registers[args.B] ? 1 : 0;

    private static void Gtri(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] > args.B ? 1 : 0;

    private static void Gtrr(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] > registers[args.B] ? 1 : 0;

    private static void Eqir(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = args.A == registers[args.B] ? 1 : 0;

    private static void Eqri(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] == args.B ? 1 : 0;

    private static void Eqrr(Arguments<int, int, int> args, int[] registers)
        => registers[args.C] = registers[args.A] == registers[args.B] ? 1 : 0;

    private static (int, Instruction[]) Parse(string input)
    {
        var lines = input.Lines().ToList();
        var ipRegister = lines[0].Extract<int>(@"#ip (\d+)");
        var program = lines
            .Skip(1)
            .Select(line => line
                .Extract(@"(.+) (\d+) (\d+) (\d+)")
                .Transform(parts =>
                    new Instruction(
                        parts.First(),
                        Arguments.Parse<int, int, int>(parts.Skip(1).Take(3).ToArray())
                        )
                    )
                )
            .ToArray();

        return (ipRegister, program);
    }
}

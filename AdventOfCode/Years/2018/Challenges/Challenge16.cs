using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Collections.Immutable;

namespace AdventOfCode2018.Challenges;

[Challenge(16)]
public class Challenge16
{
    private delegate void Operation(Arguments<int, int, int> args, int[] registers);

    private static readonly Operation[] Operations = [Addr, Addi, Mulr, Muli, Banr, Bani, Borr, Bori, Setr, Seti, Gtir, Gtri, Gtrr, Eqir, Eqri, Eqrr];

    [Part(1, "570")]
    public string Part1(string input)
    {
        var (tests, _) = Parse(input);
        return tests.Count(t => GetMatchingOperations((Arguments<int, int, int>)Arguments.Parse<int, int, int>(t.Input[1..]), t.Before, t.After).Count >= 3).ToString();
    }

    [Part(2, "503")]
    public string Part2(string input)
    {
        var (tests, program) = Parse(input);
        var operations = DetermineOperations(tests);

        var memory = new int[4];

        int ip = 0;
        while(ip >= 0 && ip < program.Length)
        {
            operations[program[ip].OpCode.As<int>()]((Arguments<int,int,int>)program[ip].Arguments, memory);
            ip++;
        }

        return memory[0].ToString();
    }

    private Operation[] DetermineOperations(Sample[] tests)
    {
        var known = new List<Operation>();
        var operations = new Operation[16];

        while(known.Count < operations.Length)
        {
            // Fill candidates with all operations that are not yet known foreach opcode
            var candidates = new List<Operation>[16];
            for(var i = 0; i < candidates.Length; i++)
                candidates[i] = [.. Operations.Except(known)];

            // Run all tests and remove any operations that do not match the before/after state for the given arguments from the candidates for the given opcode
            foreach (var test in tests)
            {
                var matches = GetMatchingOperations((Arguments<int, int, int>)Arguments.Parse<int,int, int>(test.Input[1..]), test.Before, test.After);
                foreach (var failedOperation in Operations.Except(matches))
                    candidates[test.Input[0].As<int>()].Remove(failedOperation);
            }

            // If any opcode has only one candidate left, we know that operation for that opcode, so we can add it to the known operations and remove it from the candidates for all other opcodes
            for (var i = 0; i < candidates.Length; i++)
            {
                if (candidates[i].Count == 1)
                {
                    operations[i] = candidates[i][0];
                    known.Add(operations[i]);
                }
            }
        }

        return operations;
    }

    private static bool TestOperation(Operation operation, Arguments<int,int,int> args, int[] registers, int[] expected)
    {
        // Execute operation
        operation(args, registers);

        // Verify the registers match the expected values
        return registers.SequenceEqual(expected);
    }

    private static HashSet<Operation> GetMatchingOperations(Arguments<int, int, int> args, ImmutableArray<int> registers, int[] expected)
        => [.. Operations.Where(op => TestOperation(op, args, [.. registers], expected))];

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

    private static (Sample[] TestOutput, Instruction[] Program) Parse(string input)
    {
        var nl = Environment.NewLine;
        return input
            .SplitBy($"{nl}{nl}{nl}")
            .Transform(parts =>
                (parts.First().Paragraphs().Select(ExtractTest).ToArray(),
                ExtractProgram(parts.Second())));
    }

    private static Sample ExtractTest(string input) => input
        .Lines()
        .Transform(lines => new Sample(
            ImmutableArray.Create([.. lines.First().Extract(@"Before: \[(\d), (\d), (\d), (\d)\]").As<int>()]),
            [.. lines.Second().SplitBy(" ")],
            [.. lines.Third().Extract(@"After:  \[(\d), (\d), (\d), (\d)\]").As<int>()]
            ));

    private static Instruction[] ExtractProgram(string input)
        => [.. input.Lines(line => line
            .SplitBy(" ")
            .Transform(args => new Instruction(args[0], Arguments.Parse<int, int, int>(args[1..]))))];

    private record Sample(ImmutableArray<int> Before, string[] Input, int[] After);
}

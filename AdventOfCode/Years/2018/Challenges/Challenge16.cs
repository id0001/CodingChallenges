using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(16)]
public class Challenge16
{
    [Part(1, "570")]
    public string Part1(string input)
    {
        var (tests, _) = Parse(input);
        return tests.Where(t => )

        for (var ip = 0; ip > 0;)
        {

        }
    }

    // [Part(2, "503")]
    // public string Part2(string input)
    // {
    // }

    private static void Addr(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] + registers[args.B];

    private static void Addi(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] + args.B;

    private static void Mulr(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] * registers[args.B];

    private static void Muli(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] * args.B;

    private static void Banr(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] & registers[args.B];

    private static void Bani(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] & args.B;

    private static void Borr(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] | registers[args.B];

    private static void Bori(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] | args.B;

    private static void Setr(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A];

    private static void Seti(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = args.A;

    private static void Gtir(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = args.A > registers[args.B] ? 1 : 0;

    private static void Gtri(Arguments<int, int, int> args, Dictionary<int, int> registers)
        => registers[args.C] = registers[args.A] > args.B ? 1 : 0;

    private static (IEnumerable<Sample> TestOutput, Instruction[] Program) Parse(string input)
    {
        var nl = Environment.NewLine;
        return input
            .SplitBy($"{nl}{nl}{nl}")
            .Transform(parts =>
                (parts.First().Paragraphs().Select(ExtractTest),
                ExtractProgram(parts.Second())));
    }

    private static Sample ExtractTest(string input) => input
        .Lines()
        .Transform(lines => new Sample(
            [.. lines.First().Extract(@"Before: \[(\d), (\d), (\d), (\d)\]").As<int>()],
            [.. lines.Second().SplitBy(" ").As<int>()],
            [.. lines.Third().Extract(@"After: \[(\d), (\d), (\d), (\d)\]").As<int>()]
            ));

    private static Instruction[] ExtractProgram(string input)
        => [.. input.Lines(line => line
            .SplitBy(" ")
            .Transform(args => new Instruction(args[0], Arguments.Parse<int, int, int>(args[1..]))))];

    private record Sample(int[] Before, int[] Input, int[] After);
}

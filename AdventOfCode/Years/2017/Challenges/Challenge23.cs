using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(23)]
public class Challenge23
{
    [Part(1, "3025")]
    public string Part1(string input)
    {
        var program = input.Lines(ParseLine).ToList();

        var ip = 0;
        var registers = new Dictionary<string, int>
        {
            ["a"] = 0,
            ["b"] = 0,
            ["c"] = 0,
            ["d"] = 0,
            ["e"] = 0,
            ["f"] = 0,
            ["g"] = 0,
            ["h"] = 0,
        };

        int mulCount = 0;
        while (ip >= 0 && ip < program.Count)
        {
            var arg1 = program[ip].Arguments.GetValue(1, r => registers[r]);

            switch (program[ip])
            {
                case ("set", Arguments args):
                    registers[args[0]] = arg1;
                    ip++;
                    break;
                
                case ("sub", Arguments args):
                    registers[args[0]] -= arg1;
                    ip++;
                    break;
                case ("mul", Arguments args):
                    registers[args[0]] *= arg1;
                    ip++;
                    mulCount++;
                    break;
                case ("jnz", Arguments args):
                    var arg0 = args.GetValue(0, r => registers[r]); 
                    ip += arg0 != 0 ? arg1 : 1;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return mulCount.ToString();
    }

    [Part(2, "915")]
    public string Part2(string input)
    {
        var b = 57 * 100 + 100000;
        var c = b + 17000;
        var h = 0;

        for (var i = b; i <= c; i += 17)
        {
            var d = 2;
            while (i % d != 0)
                d++;

            if (i != d)
                h++;
        }

        return h.ToString();
    }

    private static Instruction ParseLine(string line)
    {
        var split = line.SplitBy(" ");
        return new Instruction(split.First(), Arguments.Parse<int, int>(split[1..]));
    }
}

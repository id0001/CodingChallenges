using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Mathematics;

namespace AdventOfCode2016.Challenges;

[Challenge(15)]
public class Challenge15
{
    [Part(1, "203660")]
    public string Part1(string input)
    {
        var discs = input.Lines(ParseLine).ToArray();
        var moduli = discs.Select(x => (long)x.NumberOfPositions).ToArray();
        var remainders = discs.Index().Select(x => -((long)x.Item.Position + x.Index + 1)).ToArray();

        return NumberTheory.ChineseRemainderTheorem(moduli, remainders).ToString();
    }

    [Part(2, "2408135")]
    public string Part2(string input)
    {
        Disc[] discs = [..input.Lines(ParseLine).ToArray(), new Disc(0,11)];
        var remainders = discs.Index().Select(x => -((long)x.Item.Position + x.Index + 1)).ToArray();
        var moduli = discs.Select(x => (long)x.NumberOfPositions).ToArray();

        return NumberTheory.ChineseRemainderTheorem(moduli, remainders).ToString();
    }

    private static Disc ParseLine(string line) => line
        .Extract<int, int>(@"^Disc #\d has (\d+) positions; at time=0, it is at position (\d+).$")
        .Transform(d => new Disc(d.Second, d.First));

    private record Disc(int Position, int NumberOfPositions);
}

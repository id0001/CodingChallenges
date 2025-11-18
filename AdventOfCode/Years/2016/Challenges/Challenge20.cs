using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(20)]
public class Challenge20
{
    [Part(1, "4793564")]
    public string Part1(string input)
    {
        var ranges = input.Lines(ParseLine).OrderBy(x => x.Start).ToList();

        var i = 0L;
        for (var ri = 0; ri < ranges.Count; ri++)
        {
            if (i < ranges[ri].Start)
                return i.ToString();

            i = Math.Max(i, ranges[ri].End + 1);
        }

        throw new InvalidOperationException();
    }

    [Part(2, "146")]
    public string Part2(string input)
    {
        var ranges = input.Lines(ParseLine).OrderBy(x => x.Start).ToList();

        var (i, count) = (0L, 0L);
        for (var ri = 0; ri < ranges.Count; ri++)
        {
            if (i < ranges[ri].Start)
                count += ranges[ri].Start - i;

            i = Math.Max(i, ranges[ri].End + 1);
        }

        return count.ToString();
    }

    private LongRange ParseLine(string line) =>
        line.SplitBy<long, long>("-").Transform(x => new LongRange(x.First, x.Second));

    private record LongRange(long Start, long End);
}

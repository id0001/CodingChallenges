using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(10)]
public class Challenge10
{
    [Part(1, "6952")]
    public string Part1(string input)
    {
        CircularBuffer<byte> numbers = new(Enumerable.Range(0, 256).Select(x => (byte)x));
        var i = 0;
        var skip = 0;

        foreach(var length in input.SplitBy<int>(","))
        {
            Twist(numbers, i, length);
            i = (i + length + skip).Mod(numbers.Count);
            skip++;
        }

        return numbers.Take(2).Select(b => (int)b).Product().ToString();
    }

    [Part(2, "28e7c4360520718a5dc811d3942cf1fd")]
    public string Part2(string input) => string.Join(string.Empty, KnotHash.Generate(input).Select(x => x.ToHexString()));

    private static void Twist(CircularBuffer<byte> hash, int start, int length)
    {
        var (l, r) = ((int)Math.Round(length / 2d, MidpointRounding.AwayFromZero) - 1, (int)Math.Floor(length / 2d));
        for (; l >= 0 && r < length; l--, r++)
            (hash[start + l], hash[start + r]) = (hash[start + r], hash[start + l]);
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(3)]
public class Challenge03
{
    [Part(1, "17229")]
    public string Part1(string input) => input
        .Lines(line => FindLargest([.. line.Select(c => (long)c.AsInteger)], 2))
        .Sum()
        .ToString();


    [Part(2, "170520923035051")]
    public string Part2(string input) => input
        .Lines(line => FindLargest([.. line.Select(c => (long)c.AsInteger)], 12))
        .Sum()
        .ToString();

    private static long FindLargest(long[] source, int length)
    {
        long result = 0;
        int nextIndex = 0;
        for (var ri = 0; ri < length; ri++)
        {
            nextIndex = FindIndexOfMaxValue(source, nextIndex, source.Length - nextIndex - (length - 1 - ri));
            result += (long)Math.Pow(10, length - 1 - ri) * source[nextIndex];
            nextIndex++;
        }

        return result;
    }

    private static int FindIndexOfMaxValue(long[] source, int startIndex, int length)
    {
        int idx = startIndex;
        long max = source[startIndex];
        for (var i = startIndex; i < startIndex + length; i++)
        {
            if(source[i] > max)
            {
                max = source[i];
                idx = i;
            }
        }

        return idx;
    }
}

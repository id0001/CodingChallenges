using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(3)]
public class Challenge03
{
    [Part(1, "17229")]
    public string Part1(string input) => input
        .Lines(line => FindLargest([.. line.Select(c => (long)c.AsInteger)], 0, 0, 2))
        .Sum()
        .ToString();


    [Part(2, "170520923035051")]
    public string Part2(string input) => input
        .Lines(line => FindLargest([.. line.Select(c => (long)c.AsInteger)], 0, 0, 12))
        .Sum()
        .ToString();

    private static long FindLargest(long[] source, int si, int ri, int length)
    {
        if (ri >= length)
            return 0; // last digit found

        if (si > source.Length)
            return -1; // Incorrect result

        for (var d = 9; d > 0; d--)
        {
            for (var i = si; i < source.Length - ((length - 1) - ri); i++)
            {
                if (source[i] == d)
                {
                    var value = source[i] * (long)Math.Pow(10, (length - 1) - ri);
                    var result = FindLargest(source, i + 1, ri + 1, length);
                    if (result >= 0)
                        return value + result; // Largest value found
                }
            }
        }

        return -1; // Incorrect result
    }
}

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
            // Get the position of the largest digit in the array from nextIndex still leaving space for the next digits.
            nextIndex = source
                .Index()
                .Skip(nextIndex)
                .Where(x => x.Index + (length - 1 - ri) < source.Length)
                .MaxBy(x => x.Item)
                .Index;

            result += (long)Math.Pow(10, length - 1 - ri) * source[nextIndex];
            nextIndex++;
        }

        return result;
    }
}

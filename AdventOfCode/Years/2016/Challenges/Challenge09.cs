using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities;

namespace AdventOfCode2016.Challenges;

[Challenge(9)]
public class Challenge09
{
    [Part(1, "74532")]
    public string Part1(string input) => DecompressV1(input).ToString();

    [Part(2, "11558231665")]
    public string Part2(string input)
    {
        var cache = new Memoized<string, long>(DecompressV2);
        return cache.Invoke(input).ToString();
    }

    private static int DecompressV1(string compressed)
    {
        var totalLength = 0;
        for (var i = 0; i < compressed.Length; i++)
        {
            if (compressed[i] == '(')
            {
                var marker = compressed[i..(compressed.IndexOf(')', i) + 1)];
                i += marker.Length;
                var (x, y) = marker.Extract<int, int>(@"(\d+)x(\d+)");
                totalLength += x * y;
                i += x - 1;
                continue;
            }

            totalLength++;
        }

        return totalLength;
    }

    private static long DecompressV2(Memoized<string, long> cache, string compressed)
    {
        long totalLength = 0;
        for (var i = 0; i < compressed.Length; i++)
        {
            if (compressed[i] == '(')
            {
                var marker = compressed[i..(compressed.IndexOf(')', i) + 1)];
                i += marker.Length;
                var (x, y) = marker.Extract<int, int>(@"(\d+)x(\d+)");

                var str = string.Join(string.Empty, Enumerable.Repeat(compressed[i..(i + x)], y));
                totalLength += cache.Invoke(str);
                i += x - 1;
                continue;
            }

            totalLength++;
        }

        return totalLength;
    }
}

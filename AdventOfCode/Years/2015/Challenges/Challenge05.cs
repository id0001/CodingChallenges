using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode2015.Challenges;

[Challenge(5)]
public class Challenge05
{
    [Part(1, "255")]
    public string Part1(string input) => input.Lines().Count(IsNice1).ToString();

    [Part(2, "55")]
    public string Part2(string input) => input.Lines().Count(IsNice2).ToString();

    private static bool IsNice1(string source)
    {
        if (Regex.Matches(source, @"a|e|i|o|u").Count < 3)
            return false;

        if (!Regex.IsMatch(source, @"(\w)\1"))
            return false;

        if (Regex.IsMatch(source, @"ab|cd|pq|xy"))
            return false;

        return true;
    }

    private static bool IsNice2(string source) => ContainsPairThatAppearsTwice(source) && ContainsRepeatingLetterWithOneLetterCap(source);

    public static bool ContainsPairThatAppearsTwice(string source) => FilterPairs(source).GroupBy(x => x).Any(x => x.Count() >= 2);

    public static bool ContainsRepeatingLetterWithOneLetterCap(string source)
    {
        for (var i = 2; i < source.Length; i++)
            if (source[i - 2] == source[i])
                return true;

        return false;
    }

    public static IEnumerable<string> FilterPairs(string source)
    {
        string? prevPair = null;
        var lastAddedIndex = -1;
        for (var i = 1; i < source.Length; i++)
        {
            var pair = source[(i - 1)..(i + 1)];
            if (prevPair is not null && prevPair == pair && i - lastAddedIndex < 2)
                continue;

            prevPair = pair;
            lastAddedIndex = i;
            yield return pair;
        }
    }
}

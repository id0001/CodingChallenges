using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(2)]
public class Challenge02
{
    [Part(1, "23701357374")]
    public string Part1(string input) => input
        .SplitBy(",")
        .SelectMany(range => range.SplitBy<long>("-")
        .Transform(r => Range(r.First(), r.Second()).Where(i => IsInvalid(i.ToString(), 2))))
        .Sum()
        .ToString();

    [Part(2, "34284458938")]
    public string Part2(string input) => input
        .SplitBy(",")
        .SelectMany(range => range.SplitBy<long>("-")
        .Transform(r => Range(r.First(), r.Second()).Where(i => IsInvalid(i.ToString()))))
        .Sum()
        .ToString();

    private static IEnumerable<long> Range(long low, long high)
    {
        for (var i = low; i <= high; i++)
            yield return i;
    }

    private static bool IsInvalid(string number)
    {
        for (var i = 2; i <=  number.Length; i++)
        {
            if (IsInvalid(number, i))
                return true;
        }

        return false;
    }

    private static bool IsInvalid(string number, int repCount)
    {
        if (number.Length % repCount != 0)
            return false;

        if (repCount > number.Length)
            return false;

        int patternLength = number.Length / repCount;
        string pattern = number[0..patternLength];

        for (var i = patternLength; i < number.Length; i += patternLength)
        {
            if (number[i..(i + patternLength)] != pattern)
                return false;
        }

        return true;
    }
}

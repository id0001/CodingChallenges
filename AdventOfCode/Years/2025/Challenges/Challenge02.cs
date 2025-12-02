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
        .Transform(r => GetInvalidNumbersInRange(r.First(), r.Second())))
        .Sum()
        .ToString();

    [Part(2, "34284458938")]
    public string Part2(string input) => input
        .SplitBy(",")
        .SelectMany(range => range.SplitBy<long>("-")
        .Transform(r => GetInvalidNumbersInRange2(r.First(), r.Second())))
        .Sum()
        .ToString();

    private static IEnumerable<long> GetInvalidNumbersInRange(long low, long high)
    {
        for (var i = low; i <= high; i++)
        {
            var s = i.ToString();
            if (s.Length % 2 != 0)
                continue;

            var len = s.Length / 2;
            if (s[0..len] == s[len..])
                yield return i;
        }
    }

    private static IEnumerable<long> GetInvalidNumbersInRange2(long low, long high)
    {
        for (var i = low; i <= high; i++)
        {
            if (IsInvalid(i.ToString(), 2))
                yield return i;
        }
    }

    private static bool IsInvalid(string number, int div)
    {
        if (div > number.Length)
            return false;

        if (number.Length % div != 0)
            return IsInvalid(number, div + 1);

        int len = number.Length / div;
        string pattern = number[0..len];
        for (int i = len; i < number.Length; i += len)
        {
            if (number[i..(i + len)] != pattern)
                return IsInvalid(number, div + 1);
        }

        return true;
    }
}

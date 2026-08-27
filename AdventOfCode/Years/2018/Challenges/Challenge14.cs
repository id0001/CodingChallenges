using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Linq;

namespace AdventOfCode2018.Challenges;

[Challenge(14)]
public class Challenge14
{
    [Part(1, "9276422810")]
    public string Part1(string input)
    {
        var n = input.As<int>();

        var (a, b) = (0, 1);
        var list = new List<int> { 3, 7 };

        while (list.Count < n + 10)
        {
            var sum = list[a] + list[b];

            if (sum >= 10)
                list.Add(sum / 10); // First digit
            list.Add(sum % 10); // Second or only digit

            a = (a + 1 + list[a]) % list.Count;
            b = (b + 1 + list[b]) % list.Count;
        }

        return string.Concat(list[^10..]);
    }

    [Part(2, "20319117")]
    public string Part2(string input)
    {
        var digits = input.As<int>().Digits().Select(d => (int)d).ToArray();

        var (a, b) = (0, 1);
        var list = new List<int> { 3, 7 };

        while (true)
        {
            var sum = list[a] + list[b];

            if (sum >= 10)
                list.Add(sum / 10); // First digit
            list.Add(sum % 10); // Second or only digit

            a = (a + 1 + list[a]) % list.Count;
            b = (b + 1 + list[b]) % list.Count;

            if (list.Count > digits.Length && (sum >= 10 ? list[^2] : list[^1]) == digits[^1] &&
                list[^(digits.Length + 1)..^1].SequenceEqual(digits))
                return (list.Count - digits.Length - 1).ToString();
        }
    }
}

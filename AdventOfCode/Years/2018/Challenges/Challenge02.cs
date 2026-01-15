using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(2)]
public class Challenge02
{
    [Part(1, "5880")]
    public string Part1(string input)
    {
        int c2 = 0, c3 = 0;
        foreach(var charCount in input.Lines(line => line.GetCharCount()))
        {
            if (charCount.Any(c => c.Value == 2))
                c2++;

            if (charCount.Any(c => c.Value == 3))
                c3++;
        }

        return (c2 * c3).ToString();
    }

    [Part(2, "tiwcdpbseqhxryfmgkvjujvza")]
    public string Part2(string input)
    {
        var correct = input.Lines().Combinations(2).First(c => c.First().HammingDistance(c.Second()) == 1);
        return ExtractCommon(correct.First(), correct.Second());
    }

    private static string ExtractCommon(string a, string b)
        => a.Zip(b).Where(pair => pair.First == pair.Second).Select(pair => pair.First).AsString();
}

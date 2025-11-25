using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace EverybodyCodes2024.Challenges;

[Challenge(4)]
public class Quest04
{
    [Part(1, "87")]
    public string Part1(string input)
    {
        var nails = input.Lines().As<int>().ToArray();
        var min = nails.Min();
        return nails.Sum(x => x - min).ToString();
    }

    [Part(2, "894606")]
    public string Part2(string input)
    {
        var nails = input.Lines().As<int>().ToArray();
        return Calculate(nails, nails.Min()).ToString();
    }

    [Part(3, "120168130")]
    public string Part3(string input)
    {
        var nails = input.Lines().As<int>().ToArray();

        var sorted = nails.Order().ToList();
        var median = sorted[sorted.Count / 2];
        return Calculate(nails, median).ToString();
        
    }

    private static long Calculate(IEnumerable<int> nails, int height) => nails.Sum(x => (long)Math.Abs(x - height));
}

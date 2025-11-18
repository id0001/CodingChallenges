using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(13)]
public class Challenge13
{
    [Part(1, "618")]
    public string Part1(string input)
    {
        var lookup = input.Lines(ParseLine).ToDictionary(kv => (kv.From, kv.To), kv => kv.Units);
        var people = lookup.Keys.Select(k => k.From).ToHashSet();

        var mostGained = 0;
        foreach (var permutation in people.Permutations())
        {
            var gained = 0;
            foreach (var (curr, next) in permutation.CurrentAndNext(true))
                gained += lookup[(curr, next)] + lookup[(next, curr)];

            mostGained = Math.Max(mostGained, gained);
        }

        return mostGained.ToString();
    }

    [Part(2, "601")]
    public string Part2(string input)
    {
        var lookup = input.Lines(ParseLine).ToDictionary(kv => (kv.From, kv.To), kv => kv.Units);
        var people = lookup.Keys.Select(k => k.From).ToHashSet();
        foreach (var person in people)
        {
            lookup.Add(("Me", person), 0);
            lookup.Add((person, "Me"), 0);
        }

        people.Add("Me");

        var mostGained = 0;
        foreach (var permutation in people.Permutations())
        {
            var gained = 0;
            foreach (var (curr, next) in permutation.CurrentAndNext(true))
                gained += lookup[(curr, next)] + lookup[(next, curr)];

            mostGained = Math.Max(mostGained, gained);
        }

        return mostGained.ToString();
    }

    private static (string From, string To, int Units) ParseLine(string line) => line
        .Extract<string, string, int, string>(@"(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+)\.")
        .Transform(value => (value.First, value.Fourth, (value.Second == "gain" ? 1 : -1) * value.Third));
}

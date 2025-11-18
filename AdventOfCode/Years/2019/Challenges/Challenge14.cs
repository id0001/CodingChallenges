using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Collections.Immutable;

namespace AdventOfCode2019.Challenges;

[Challenge(14)]
public class Challenge14
{
    [Part(1, "1046184")]
    public string Part1(string input)
    {
        var reactions = input.Lines(ParseLine).ToDictionary(kv => kv.Output);

        var ore = CalculateOreNeeds(reactions, "FUEL", 1, []);
        return ore.ToString();
    }

    [Part(2, "1639374")]
    public string Part2(string input)
    {
        var reactions = input.Lines(ParseLine).ToDictionary(kv => kv.Output);

        // Do a binary search to find the max produced fuel amount for 1 trillion ore.

        var lo = 0L;
        var hi = 1_000_000_000_000L;

        const long expected = 1_000_000_000_000L;

        while (hi - lo > 1L)
        {
            var mid = (long)Math.Floor((hi + lo) / 2d);
            var cost = CalculateOreNeeds(reactions, "FUEL", mid, []);
            if (cost > expected)
                hi = mid;
            else
                lo = mid;
        }

        return lo.ToString();
    }

    private long CalculateOreNeeds(Dictionary<string, Reaction> reactions, string component, long amount, Dictionary<string, long> supply)
    {
        supply.TryAdd(component, 0);

        // If the supply has enough, don't make new components
        if (supply[component] >= amount)
        {
            supply[component] -= amount;
            return 0;
        }

        // Take from the supply and make the rest
        amount -= supply[component];
        supply[component] = 0;

        // If the componet is ore, just return the amount to make
        if (component == "ORE")
            return amount;

        var reaction = reactions[component];

        var neededReactions = (long)Math.Ceiling(amount / (double)reaction.AmountProduced);
        var oreNeeded = reaction.Input.Sum(kv => CalculateOreNeeds(reactions, kv.Key, kv.Value * neededReactions, supply));
        var leftOver = neededReactions * reaction.AmountProduced - amount;
        supply[component] = leftOver;
        return oreNeeded;
    }

    private static Reaction ParseLine(string line)
    {
        var (input, output) = line.SplitBy<string, string>("=>");
        var inputs = input.SplitBy(",");
        var (outAmount, outType) = output.SplitBy<int, string>(" ");

        return new Reaction(outType, outAmount, inputs.Select(x => x.SplitBy<int, string>(" ")).ToImmutableDictionary(kv => kv.Second, kv => kv.First));
    }

    private record Reaction(string Output, int AmountProduced, ImmutableDictionary<string, int> Input);
}

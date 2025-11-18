using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(9)]
public class Challenge09
{
    [Part(1, "251")]
    public string Part1(string input)
    {
        var routes = input.Lines().SelectMany(ParseLine).ToDictionary(kv => (kv.From, kv.To), kv => kv.Distance);
        var cities = routes.Keys.Select(x => x.From).Distinct().ToList();
        return cities.Permutations().Min(p => CalculateDistance(p, routes)).ToString();
    }

    [Part(2, "898")]
    public string Part2(string input)
    {
        var routes = input.Lines().SelectMany(ParseLine).ToDictionary(kv => (kv.From, kv.To), kv => kv.Distance);
        var cities = routes.Keys.Select(x => x.From).Distinct().ToList();
        return cities.Permutations().Max(p => CalculateDistance(p, routes)).ToString();
    }

    private static IEnumerable<(string From, string To, int Distance)> ParseLine(string line)
    {
        var (a, b, distance) = line.Extract<string, string, int>(@"(.+) to (.+) = (\d+)");
        yield return (a, b, distance);
        yield return (b, a, distance);
    }

    public static int CalculateDistance(IList<string> source, IDictionary<(string, string), int> routesLookup)
       => source.Windowed(2).Aggregate(0, (dist, window) => dist + routesLookup[(window.First(), window.Second())]);
}

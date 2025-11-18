using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(16)]
public class Challenge16
{
    private static readonly (string, int, Func<int, int, bool>)[] Signature =
    {
        ("children", 3, (a, b) => a == b),
        ("cats", 7, (a, b) => a > b),
        ("samoyeds", 2, (a, b) => a == b),
        ("pomeranians", 3, (a, b) => a < b),
        ("akitas", 0, (a, b) => a == b),
        ("vizslas", 0, (a, b) => a == b),
        ("goldfish", 5, (a, b) => a < b),
        ("trees", 3, (a, b) => a > b),
        ("cars", 2, (a, b) => a == b),
        ("perfumes", 1, (a, b) => a == b)
    };

    [Part(1, "40")]
    public string Part1(string input) => input
        .Lines(ParseLine)
        .OrderByDescending(s => s.MatchCount1(Signature.Select(x => x.Item2)))
        .First()
        .Number
        .ToString();

    [Part(2, "241")]
    public string Part2(string input) => input
        .Lines(ParseLine)
        .OrderByDescending(s => s.MatchCount2(Signature))
        .First()
        .Number
        .ToString();

    private static Sue ParseLine(string line)
    {
        var (number, itemStr) = line.Extract<int, string>(@"Sue (\d+):(.+)");

        var values = new int[10];
        Array.Fill(values, -1);

        foreach (var item in itemStr.SplitBy(","))
        {
            var (key, value) = item.SplitBy<string, int>(":");
            var i = Array.FindIndex(Signature, ((string Name, int, Func<int, int, bool>) x) => x.Name == key);
            values[i] = value;
        }

        return new Sue(number, values);
    }

    private record Sue(int Number, int[] Values)
    {
        public int MatchCount1(IEnumerable<int> expected) => Values.Zip(expected).Count(x => x.First == x.Second);

        public int MatchCount2(IEnumerable<(string Name, int Expected, Func<int, int, bool> Compare)> signature)
            => Values.Zip(signature).Count(x => x.First != -1 && x.Second.Compare(x.First, x.Second.Expected));
    }
}

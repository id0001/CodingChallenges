using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Mathematics;

namespace AdventOfCode2015.Challenges;

[Challenge(15)]
public class Challenge15
{
    [Part(1, "13882464")]
    public string Part1(string input)
    {
        var list = input.Lines(ParseLine).ToList();

        return Combinatorics.GeneratePartitions(100, list.Count)
            .Select(dist =>
            {
                var zip = list.Zip(dist).ToList();
                var cap = Math.Max(0, zip.Sum(x => x.First.Capacity * x.Second));
                var dur = Math.Max(0, zip.Sum(x => x.First.Durability * x.Second));
                var fla = Math.Max(0, zip.Sum(x => x.First.Flavor * x.Second));
                var tex = Math.Max(0, zip.Sum(x => x.First.Texture * x.Second));
                return cap * dur * fla * tex;
            })
            .Max()
            .ToString();
    }

    [Part(2, "11171160")]
    public string Part2(string input)
    {
        var list = input.Lines(ParseLine).ToList();

        return Combinatorics.GeneratePartitions(100, list.Count)
            .Select(dist =>
            {
                var zip = list.Zip(dist).ToList();
                var cap = Math.Max(0, zip.Sum(x => x.First.Capacity * x.Second));
                var dur = Math.Max(0, zip.Sum(x => x.First.Durability * x.Second));
                var fla = Math.Max(0, zip.Sum(x => x.First.Flavor * x.Second));
                var tex = Math.Max(0, zip.Sum(x => x.First.Texture * x.Second));
                var cal = Math.Max(0, zip.Sum(x => x.First.Calories * x.Second));
                return (Calories: cal, Score: cap * dur * fla * tex);
            })
            .Where(x => x.Calories == 500)
            .Max(x => x.Score)
            .ToString();
    }

    private static Ingredient ParseLine(string line) => line
        .Extract(@"\w+: capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)")
        .As<int>()
        .Transform(match => new Ingredient(match.First(), match.Second(), match.Third(), match.Fourth(), match.Fifth()));

    private record Ingredient(int Capacity, int Durability, int Flavor, int Texture, int Calories);
}

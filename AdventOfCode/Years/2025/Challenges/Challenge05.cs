using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(5)]
public class Challenge05
{
    [Part(1, "511")]
    public string Part1(string input)
    {
        var (fresh, ingredients) = ParseInput(input);

        return ingredients
            .Where(i => fresh.Any(r => r.IsInRange(i)))
            .Count()
            .ToString();
    }

    [Part(2, "350939902751909")]
    public string Part2(string input)
    {
        var (fresh, _) = ParseInput(input);

        return fresh
            .MergeBy(IngredientRange.HasOverlap, IngredientRange.Merge)
            .Sum(r => r.Length)
            .ToString();
    }

    private static (IEnumerable<IngredientRange> FreshRanges, long[] Ingredients) ParseInput(string input)
    {
        return input
            .Paragraphs()
            .Transform(paragraphs => (
                paragraphs.First().Lines(line => line.SplitBy<long>("-").Transform(r => new IngredientRange(r.First(), r.Second()))),
                paragraphs.Second().Lines().Select(line => line.As<long>()).ToArray()));
    }

    private record IngredientRange(long Start, long End)
    {
        public long Length = (End + 1 - Start);

        public bool IsInRange(long value) => value >= Start && value < Start + Length;

        public static IngredientRange Merge(IngredientRange a, IngredientRange b) 
            => new IngredientRange(Math.Min(a.Start, b.Start), Math.Max(a.End, b.End));

        public static bool HasOverlap(IngredientRange a, IngredientRange b) => b.End >= a.Start && b.Start <= a.End;
    }
}

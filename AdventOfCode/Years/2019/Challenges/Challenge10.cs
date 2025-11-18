using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Comparers;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(10)]
public class Challenge10
{
    //[Part(1, "280")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();
        var astroids = grid.Where(x => x.Value == '#').Select(x => x.Key).ToList();

        return astroids
            .Max(a => astroids.Where(b => b != a)
                .GroupBy(b => Vector2.AngleOnCircle(Vector2.Zero, b.ToVector2() - a.ToVector2()), DoubleEqualityComparer.Default)
                .Count()
            )
            .ToString();
    }

    [Part(2, "706")]
    public string Part2(string input)
    {
        var grid = input.ToGrid();
        var astroids = grid.Where(x => x.Value == '#').Select(x => x.Key).ToList();

        var center = astroids
           .MaxBy(a => astroids.Where(b => b != a)
               .GroupBy(b => Vector2.AngleOnCircle(Vector2.Zero, b.ToVector2() - a.ToVector2()), DoubleEqualityComparer.Default)
               .Count()
           );

        var sorted = astroids
            .Where(b => b != center)
            .GroupBy(b => Vector2.AngleOnCircle(Vector2.Zero, b.ToVector2() - center.ToVector2(), Math.PI / 2d), DoubleEqualityComparer.Default)
            .OrderBy(g => g.Key)
            .Select(g => g.OrderBy(b => Vector2.DistanceSquared(center.ToVector2(), b.ToVector2())).ToList())
            .ToList();

        var p = EnumerateSortedAstroids(sorted).Skip(199).First();
        return (p.X * 100 + p.Y).ToString();
    }

    private static IEnumerable<Point2> EnumerateSortedAstroids(List<List<Point2>> sortedList)
    {
        var totalLength = sortedList.Sum(x => x.Count);
        var indices = new int[sortedList.Count];
        var i = 0;
        for (var k = 0; k < totalLength; k++)
        {
            if (indices[i] < sortedList[i].Count)
            {
                yield return sortedList[i][indices[i]];
                indices[i]++;
            }

            i = (i + 1).Mod(sortedList.Count);
        }
    }
}

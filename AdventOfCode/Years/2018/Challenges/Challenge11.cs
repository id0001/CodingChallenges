using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Utilities;

namespace AdventOfCode2018.Challenges;

[Challenge(11)]
public class Challenge11
{
    [Part(1, "20,46")]
    public string Part1(string input)
    {
        var sat = new SummedAreaTable<int>(PrecalculatePowerlevels(input.As<int>()));
        var largest = Range(297, 297).MaxBy(p => sat.SumQuery(p.X, p.Y, 3, 3));
        return $"{largest.X},{largest.Y}";
    }

    [Part(2, "231,65,14")]
    public string Part2(string input)
    {
        var sat = new SummedAreaTable<int>(PrecalculatePowerlevels(input.As<int>()));
        var largest = Enumerable.Range(1, 301)
            .SelectMany(size => Range(301 - size, 301 - size).Select(p => new Point3(p.X, p.Y, size)))
            .MaxBy(p => sat.SumQuery(p.X, p.Y, p.Z, p.Z));

        return $"{largest.X},{largest.Y},{largest.Z}";
    }

    private static IEnumerable<Point2> Range(int width, int height)
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                yield return new Point2(x, y);
            }
        }
    }

    private static int GetPowerlevel(Point2 p, int serialNumber) =>
        ((GetRackId(p.X) * p.Y + serialNumber) * GetRackId(p.X)).DigitAt(2) - 5;

    private static int GetRackId(int x) => x + 10;

    private static Grid2<int> PrecalculatePowerlevels(int serialNumber) => Grid2.Fill(300, 300, p => GetPowerlevel(p, serialNumber));
}

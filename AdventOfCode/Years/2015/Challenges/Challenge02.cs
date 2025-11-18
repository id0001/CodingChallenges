using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(2)]
public class Challenge02
{
    [Part(1, "1598415")]
    public string Part1(string input) =>
        input.Lines(ParseLine).Sum(TotalPaperNeeded).ToString();

    [Part(2, "3812909")]
    public string Part2(string input) =>
        input.Lines(ParseLine).Sum(TotalRibbonNeeded).ToString();

    private static Cube ParseLine(string line) => line
        .SplitBy<int>("x")
        .Order()
        .Transform(x => new Cube(0, 0, 0, x.First(), x.Second(), x.Third()));

    private static int TotalPaperNeeded(Cube cube) => cube.TotalSurfaceArea + cube.SmallestArea;

    private static int TotalRibbonNeeded(Cube cube) => cube.SmallestPerimeter + cube.Volume;
}

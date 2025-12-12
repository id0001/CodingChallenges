using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(12)]
public class Challenge12
{
    [Part(1)]
    public string Part1(string input)
    {
        var paragraphs = input.Paragraphs().ToArray();
        var regions = paragraphs[^1].Lines(ParseRegion).ToArray();

        return regions.Where(r => r.Width * r.Height >= r.Presents.Sum() * 7).Count().ToString();
    }

    private static Region ParseRegion(string line)
    {
        var (w, h, rest) = line.Extract<int, int, string>(@"(\d+)x(\d+): (.+)");
        return new Region(w, h, rest.SplitBy<int>(" ").ToArray());
    }

    private record Region(int Width, int Height, int[] Presents);
}

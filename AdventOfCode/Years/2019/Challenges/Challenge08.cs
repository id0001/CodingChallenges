using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(8)]
public class Challenge08
{
    private const int Width = 25;
    private const int Height = 6;

    [Part(1, "1677")]
    public string Part1(string input) => input
        .Chunk(Width * Height)
        .MinBy(c => c.Count(c => c == '0'))!
        .Transform(x => x.Count(c => c == '1') * x.Count(c => c == '2'))
        .ToString();

    [Part(2, "UBUFP")]
    public string Part2(string input)
    {
        var layers = input.Chunk(Width * Height).ToArray();
        var result = new Grid2<bool>(Height,Width).Fill(c =>
        {
            int index = c.Y * Width + c.X;
            return layers.SkipWhile(c => c[index] == '2').First()[index] == '1';
        });

        return result.Ocr();
    }
}

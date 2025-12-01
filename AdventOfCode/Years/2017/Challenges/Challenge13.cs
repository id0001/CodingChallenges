using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(13)]
public class Challenge13
{
    [Part(1, "1612")]
    public string Part1(string input) => input
        .Lines(ParseLine)
        .Aggregate(0, (score, shell) => score + (GetPositionInTime(shell.Layer, shell.Depth) == 0 ? shell.Layer * shell.Depth : 0))
        .ToString();

    [Part(2, "3907994")]
    public string Part2(string input)
    {
        var shells = input.Lines(ParseLine).ToList();
        return Enumerable.Range(0, int.MaxValue)
            .Where(delay => !shells.Any(shell => GetPositionInTime(delay + shell.Layer, shell.Depth) == 0))
            .First()
            .ToString();
    }

    private static (int Layer, int Depth) ParseLine(string line) => line
        .SplitBy<int>(":")
        .Transform(parts => (parts.First(), parts.Second()));

    private static int GetPositionInTime(int time, int depth)
    {
        var seqlen = depth + (depth - 2);
        var pos = time % seqlen;
        return pos >= depth ? seqlen - pos : pos;
    }
}

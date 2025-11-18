using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(3)]
public class Challenge03
{
    [Part(1, "1050")]
    public string Part1(string input) => input
        .Lines(line => line.SplitBy(" ").As<int>())
        .Count(arr =>
            arr.First() + arr.Second() > arr.Third()
            && arr.Second() + arr.Third() > arr.First()
            && arr.Third() + arr.First() > arr.Second())
        .ToString();

    [Part(2, "1921")]
    public string Part2(string input) => input
        .Lines(line => line.SplitBy(" ").As<int>())
        .Transform(lines => lines.Select(l => l.First()).Concat(lines.Select(l => l.Second()).Concat(lines.Select(l => l.Third()))))
        .Chunk(3)
        .Count(arr =>
            arr.First() + arr.Second() > arr.Third()
            && arr.Second() + arr.Third() > arr.First()
            && arr.Third() + arr.First() > arr.Second())
        .ToString();
}

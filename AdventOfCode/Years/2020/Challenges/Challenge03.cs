using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2020.Challenges;

[Challenge(3)]
public class Challenge03
{
    [Part(1, "178")]
    public string Part1(string input) => TraverseSlope([.. input.Lines()], 3, 1).ToString();

    [Part(2, "3492520200")]
    public string Part2(string input)
    {
        var lines = input.Lines().ToList();

        return (TraverseSlope(lines, 1, 1) *
                TraverseSlope(lines, 3, 1) *
                TraverseSlope(lines, 5, 1) *
                TraverseSlope(lines, 7, 1) *
                TraverseSlope(lines, 1, 2)).ToString();
    }


    private static long TraverseSlope(List<string> input, int dx, int dy)
    {
        var x = 0;
        var y = 0;

        var treeCount = 0;
        while (y < input.Count)
        {
            if (input[y][x] == '#')
                treeCount++;

            x = (x + dx) % input[0].Length;
            y += dy;
        }

        return treeCount;
    }
}

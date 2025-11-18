using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Mathematics;

namespace AdventOfCode2015.Challenges;

[Challenge(25)]
public class Challenge25
{
    [Part(1, "2650453")]
    public string Part1(string input)
    {
        const int r = 2978;
        const int c = 3083;

        var n = 20151125L;

        var ix = GetIndex(r - 1, c - 1);
        for (var i = 0; i < ix; i++) n = (n * 252_533L).Mod(33_554_393L);

        return n.ToString();
    }

    private static int GetIndex(int r, int c) => NumberTheory.TriangularNumber(r + c) + c;
}

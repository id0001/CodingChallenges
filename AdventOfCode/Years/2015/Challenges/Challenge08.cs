using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(8)]
public class Challenge08
{
    [Part(1, "1371")]
    public string Part1(string input) => input
        .Lines()
        .Aggregate(
            (A: 0, B: 0),
            (acc, line) => ParseLine(line)
                .Transform(t => (acc.A + t.A, acc.B + t.B))
            ).Transform(t => t.A - t.B)
        .ToString();

    [Part(2, "2117")]
    public string Part2(string input) => input
        .Lines()
        .Aggregate(
            (A: 0, B: 0),
            (acc, line) => (acc.A + line.Length + line.Count(c => c is '"' or '\\') + 2, acc.B + line.Length)
            ).Transform(t => t.A - t.B)
        .ToString();

    private static (int A, int B) ParseLine(string line)
    {
        var c1 = line.Length;
        var c2 = 0;
        var escaped = false;
        for (var i = 1; i < line.Length - 1; i++)
        {
            if (line[i] == '\\' && !escaped)
            {
                escaped = true;
                continue;
            }

            if (escaped && line[i] == 'x') i += 2;

            c2++;
            escaped = false;
        }

        return (c1, c2);
    }
}

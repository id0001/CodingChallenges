using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(1)]
public class Challenge01
{
    [Part(1, "1147")]
    public string Part1(string input)
    {
        int sum = 0;
        int value = 50;

        foreach (var line in input.Lines())
        {
            var d = line[0];
            var v = line[1..].As<int>();

            value = d switch
            {
                'L' => (value - v).Mod(100),
                'R' => (value + v).Mod(100),
                _ => value
            };

            if (value == 0)
                sum++;
        }

        return sum.ToString();
    }

    [Part(2, "6789")]
    public string Part2(string input)
    {
        int sum = 0;
        int value = 50;

        foreach (var line in input.Lines())
        {
            var d = line[0];
            var v = line[1..].As<int>();

            for (var i = 0; i < v; i++)
            {
                value = (value + (d == 'R' ? 1 : -1)).Mod(100);

                if (value == 0)
                    sum++;
            }
        }

        return sum.ToString();
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2020.Challenges;

[Challenge(1)]
public class Challenge01
{
    [Part(1, "935419")]
    public string Part1(string input)
    {
        var numbers = input.Lines().As<int>().ToArray();

        for (var y = 0; y < numbers.Length; y++)
            for (var x = 0; x < numbers.Length; x++)
                if (numbers[x] + numbers[y] == 2020)
                    return (numbers[x] * numbers[y]).ToString();

        throw new InvalidOperationException("No solution found.");
    }

    [Part(2, "49880012")]
    public string Part2(string input)
    {
        var numbers = input.Lines().As<int>().ToArray();

        for (var z = 0; z < numbers.Length; z++)
            for (var y = 0; y < numbers.Length; y++)
                for (var x = 0; x < numbers.Length; x++)
                {
                    if (x == y || x == z || y == z)
                        continue;

                    if (numbers[x] + numbers[y] + numbers[z] == 2020)
                        return (numbers[x] * numbers[y] * numbers[z]).ToString();
                }

        throw new InvalidOperationException("No solution found.");
    }
}

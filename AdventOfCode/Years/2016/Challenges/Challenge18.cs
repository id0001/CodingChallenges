using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Core;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(18)]
public class Challenge18
{
    [Part(1, "1982")]
    public string Part1(string input)
    {
        var line = input.ToCharArray();

        var safeCount = input.Count(c => c == '.');
        for (var row = 1; row < 40; row++)
        {
            line = Enumerable.Range(0, line.Length).Select(i =>
            {
                var leftTrap = i > 0 && line[i - 1] == '^';
                var rightTrap = i < line.Length - 1 && line[i + 1] == '^';

                return leftTrap ^ rightTrap ? '^' : '.';
            }).ToArray();
            safeCount += line.Count(c => c == '.');
        }

        return safeCount.ToString();
    }

    [Part(2, "20005203")]
    public string Part2(string input)
    {
        var line = input.ToCharArray();

        var safeCount = line.Count(c => c == '.');
        for (var row = 1; row < 400000; row++)
        {
            line = Enumerable.Range(0, line.Length).Select(i =>
            {
                var leftTrap = i > 0 && line[i - 1] == '^';
                var rightTrap = i < line.Length - 1 && line[i + 1] == '^';

                return leftTrap ^ rightTrap ? '^' : '.';
            }).ToArray();
            safeCount += line.Count(c => c == '.');
        }

        return safeCount.ToString();
    }
}

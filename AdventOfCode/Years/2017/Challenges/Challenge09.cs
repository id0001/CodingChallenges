using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventOfCode2017.Challenges;

[Challenge(9)]
public class Challenge09
{
    [Part(1, "9662")]
    public string Part1(string input)
    {
        var score = 0;
        var depth = 0;
        var inGarbage = false;
        var shouldIgnore = false;
        for (var i = 0; i < input.Length; i++)
        {
            (score, depth, inGarbage, shouldIgnore) = (input[i], inGarbage, shouldIgnore) switch
            {
                ('<', false, false) => (score, depth, true, false),
                ('>', true, false) => (score, depth, false, false),
                ('{', false, false) => (score, depth + 1, inGarbage, false),
                ('}', false, false) => (score + depth, depth - 1, inGarbage, false),
                ('!', _, false) => (score, depth, inGarbage, true),
                _ => (score, depth, inGarbage, false)
            };
        }

        return score.ToString();
    }

    [Part(2, "4903")]
    public string Part2(string input)
    {
        var score = 0;
        var inGarbage = false;
        var shouldIgnore = false;
        for (var i = 0; i < input.Length; i++)
        {
            (score, inGarbage, shouldIgnore) = (input[i], inGarbage, shouldIgnore) switch
            {
                ('<', false, false) => (score, true, false),
                ('>', true, false) => (score, false, false),
                ('{', false, false) => (score, inGarbage, false),
                ('}', false, false) => (score, inGarbage, false),
                ('!', _, false) => (score, inGarbage, true),
                (_, true, false) => (score + 1, inGarbage, false),
                _ => (score, inGarbage, false)
            };
        }

        return score.ToString();
    }
}

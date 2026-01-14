using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(25)]
public class Challenge25
{
    [Part(1, "4387")]
    public string Part1(string input)
    {
        var (steps, rules) = input.Paragraphs().Transform(paragraphs => 
        (
            paragraphs.First().Extract<int>(@"checksum after (\d+) steps.$"),
            ParseRules(paragraphs.Skip(1))
        ));

        var currentState = "A";
        var cursor = 0;
        var tape = new Dictionary<int, int>();

        for(var i = 0; i < steps; i++)
        {
            var rule = rules[currentState];
            var v = tape.GetValueOrDefault(cursor, 0);
            if(v == 0)
            {
                tape[cursor] = rule.WriteZero;
                cursor += rule.MoveZero == "left" ? -1 : 1;
                currentState = rule.NextZero;
            }
            else
            {
                tape[cursor] = rule.WriteOne;
                cursor += rule.MoveOne == "left" ? -1 : 1;
                currentState = rule.NextOne;
            }
        }

        return tape.Count(kv => kv.Value == 1).ToString();
    }

    private static Dictionary<string, Rule> ParseRules(IEnumerable<string> paragraphs)
    {
        return paragraphs.Select(x => x.Lines().ToList().Transform(lines =>
        {
            var state = lines[0].Extract<string>(@"In state (.):");
            var writeZero = lines[2].Extract<int>(@"Write the value (\d).");
            var moveZero = lines[3].Extract<string>(@"(left|right)");
            var nextZero = lines[4].Extract<string>(@"Continue with state (.)\.");

            var writeOne = lines[6].Extract<int>(@"Write the value (\d).");
            var moveOne = lines[7].Extract<string>(@"(left|right)");
            var nextOne = lines[8].Extract<string>(@"Continue with state (.)\.");

            return new Rule(state, writeZero, moveZero, nextZero, writeOne, moveOne, nextOne);
        })).ToDictionary(kv => kv.State);
    }

    private record Rule(
        string State,
        int WriteZero,
        string MoveZero,
        string NextZero,
        int WriteOne,
        string MoveOne,
        string NextOne);
}

using CodingChallenge.Utilities.Attributes;
using System.Text;

namespace AdventOfCode2015.Challenges;

[Challenge(10)]
public class Challenge10
{
    [Part(1, "252594")]
    public string Part1(string input) => Enumerable.Range(0, 40).Aggregate(input, (r, _) => Process(r)).Length.ToString();

    [Part(2, "3579328")]
    public string Part2(string input) => Enumerable.Range(0, 50).Aggregate(input, (r, _) => Process(r)).Length.ToString();

    public static string Process(string input)
    {
        var stringBuilder = new StringBuilder();

        var count = 1;
        var type = input[0];
        for (var i = 1; i < input.Length; i++)
        {
            if (input[i] == type)
            {
                count++;
                continue;
            }

            stringBuilder.Append(count);
            stringBuilder.Append(type);
            type = input[i];
            count = 1;
        }

        stringBuilder.Append(count);
        stringBuilder.Append(type);
        return stringBuilder.ToString();
    }
}
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Challenges;

[Challenge(2)]
public class Challenge02
{
    [Part(1, "434")]
    public string Part1(string input)
    {
        var lines = input.Lines(ParseLine).ToList();
        return lines
            .Where(line => line.MatchCount >= line.Min && line.MatchCount <= line.Max)
            .Select(line => line.Password)
            .Count()
            .ToString();
    }

    [Part(2, "509")]
    public string Part2(string input)
    {
        var lines = input.Lines(ParseLine).ToList();
        return lines
            .Where(line => (line.Password[line.Min - 1] == line.Character) ^ (line.Password[line.Max - 1] == line.Character))
            .Select(line => line.Password)
            .Count()
            .ToString();
    }

    private static InputLine ParseLine(string line) => line
        .Extract<int,int,char, string>(@"^(\d+)-(\d+) (\w): (\w+)$")
        .Transform(res => new InputLine(res.First, res.Second, res.Third, res.Fourth));

    private record InputLine(int Min, int Max, char Character, string Password)
    {
        public int MatchCount { get; } = Regex.Count(Password, Character.ToString());
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Challenges;

[Challenge(4)]
public class Challenge04
{
    private static readonly string[] EyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

    [Part(1, "230")]
    public string Part1(string input) => input
        .Paragraphs()
        .Select(Parse)
        .Count(x => x.HasRequiredFields)
        .ToString();

    [Part(2, "156")]
    public string Part2(string input) => input
        .Paragraphs()
        .Select(Parse)
        .Count(x => x.IsValid)
        .ToString();

    private static Passport Parse(string input) => new(input
        .SplitBy(" ", Environment.NewLine)
        .Select(x => x.SplitBy(":"))
        .ToDictionary(x => x[0], x => x[1]));

    private record Passport(Dictionary<string, string> Fields)
    {
        public bool HasRequiredFields => Fields.Count == 8 || (Fields.Count == 7 && !Fields.ContainsKey("cid"));

        public bool IsValid
        {
            get
            {
                if (!HasRequiredFields)
                    return false;

                if (Fields["byr"].Length != 4 || !int.TryParse(Fields["byr"], out var byr) || byr < 1920 || byr > 2002)
                    return false;

                if (Fields["iyr"].Length != 4 || !int.TryParse(Fields["iyr"], out var iyr) || iyr < 2010 || iyr > 2020)
                    return false;

                if (Fields["eyr"].Length != 4 || !int.TryParse(Fields["eyr"], out var eyr) || eyr < 2020 || eyr > 2030)
                    return false;

                if (!Fields["hgt"].TryExtract<int, string>(@"(\d+)(cm|in)", out var hgt, out var unit))
                    return false;

                switch (unit)
                {
                    case "cm" when hgt < 150 || hgt > 193:
                    case "in" when hgt < 59 || hgt > 76:
                        return false;
                }

                if (!Regex.IsMatch(Fields["hcl"], @"^#[0-9a-f]{6}$"))
                    return false;

                if(!EyeColors.Contains(Fields["ecl"]))
                    return false;

                return Fields["pid"].Length == 9 && int.TryParse(Fields["pid"], out _);
            }
        }
    }
}

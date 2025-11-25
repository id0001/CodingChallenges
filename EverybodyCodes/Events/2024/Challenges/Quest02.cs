using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace EverybodyCodes2024.Challenges;

[Challenge(2)]
public class Quest02
{
    [Part(1, "31")]
    public string Part1(string input)
    {
        var (runes, line) = input.Paragraphs().Transform(parts => (parts.First().Substring(6).SplitBy(","), parts.Second()));

        int sum = 0;
        foreach (var rune in runes)
        {
            var matches = Regex.Matches(line, $@"({rune})");
            if (matches.Any())
                sum += matches.Count;
        }

        return sum.ToString();
    }

    [Part(2, "5345")]
    public string Part2(string input)
    {
        var (runes, symbols) = input
            .Paragraphs()
            .Transform(parts => (
                parts.First().Substring(6).SplitBy(","),
                parts.Second().Lines(ParseToSymbols).ToArray()
                )
            );

        foreach (var rune in runes.Concat(runes.Select(r => r.Reverse().AsString())))
        {
            for (var y = 0; y < symbols.Length; y++)
            {
                for (var x = 0; x < symbols[y].Length; x++)
                    Mark(symbols[y], x, rune);
            }
        }

        return symbols.Sum(row => row.Count(x => x.IsMarked)).ToString();
    }


    [Part(3, "11884")]
    public string Part3(string input)
    {
        var (runes, symbols) = input
            .Paragraphs()
            .Transform(parts => (
                parts.First().Substring(6).SplitBy(","),
                new Grid2<Symbol>(parts.Second().Lines(ParseToSymbols).ToArray().To2dArray())
                )
            );

        foreach (var rune in runes.Concat(runes.Select(r => r.Reverse().AsString())))
        {
            for (var y = 0; y < symbols.Rows; y++)
            {
                var row = symbols.Row(y).ToArray();
                for (var x = 0; x < symbols.Columns; x++)
                    Mark(row, x, rune, true);
            }

            for (var x = 0; x < symbols.Columns; x++)
            {
                var column = symbols.Column(x).ToArray();
                for (var y = 0; y < symbols.Rows; y++)
                    Mark(column, y, rune);
            }
        }

        return symbols.Values.Count(s => s.IsMarked).ToString();
    }

    private static bool Mark(Symbol[] source, int index, string rune, bool wrap = false)
    {
        if (rune.Length == 0)
            return true;

        if (index >= source.Length)
            return false;

        if (source[index].Character != rune[0])
            return false;

        var nextIndex = wrap ? (index + 1).Mod(source.Length) : index + 1;
        var result = Mark(source, nextIndex, rune[1..]);
        if (result)
            source[index].IsMarked = true;

        return result;
    }

    private Symbol[] ParseToSymbols(string line) => [.. line.Select(c => new Symbol(c))];

    private class Symbol(char character)
    {
        public char Character => character;
        public bool IsMarked { get; set; }
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Text;

namespace AdventOfCode2016.Challenges;

[Challenge(21)]
public class Challenge21
{
    [Part(1, "ghfacdbe")]
    public string Part1(string input) => input.Lines(l => ParseLine(l)).Aggregate(new StringBuilder("abcdefgh"), (p, o) => o(p)).ToString();

    [Part(2, "fhgcdaeb")]
    public string Part2(string input) => input.Lines(l => ParseLine(l, true)).Reverse().Aggregate(new StringBuilder("fbgdceah"), (p, o) => o(p)).ToString();

    private Func<StringBuilder, StringBuilder> ParseLine(string line, bool isPart2 = false)
    {
        if (line.TryExtract<int, int>(@"swap position (\d)+ with position (\d)+", out var sp1, out var sp2))
            return s => SwapPositions(s, sp1, sp2);

        if (line.TryExtract<char, char>(@"swap letter (\w) with letter (\w)", out var sl1, out var sl2))
            return s => SwapLetters(s, sl1, sl2);

        if (line.TryExtract<string, int>(@"rotate (right|left) (\d+) steps?", out var r1, out var r2))
            return s => Rotate(s,
                r1 == "left" ? (isPart2 ? 1 : -1) * r2 : (isPart2 ? -1 : 1) * r2);

        if (line.TryExtract<char>(@"rotate based on position of letter (\w)", out var rlr1))
            return isPart2 ? s => RotateFromLetterReversed(s, rlr1) : s => RotateFromLetter(s, rlr1);

        if (line.TryExtract<int, int>(@"reverse positions (\d+) through (\d+)", out var rv1, out var rv2))
            return s => Reverse(s, rv1, rv2);

        if (line.TryExtract<int, int>(@"move position (\d+) to position (\d+)", out var m1, out var m2))
            return isPart2 ? s => Move(s, m2, m1) : s => Move(s, m1, m2);

        throw new NotImplementedException();
    }

    private static StringBuilder SwapPositions(StringBuilder password, int a, int b)
    {
        (password[b], password[a]) = (password[a], password[b]);
        return password;
    }

    private static StringBuilder SwapLetters(StringBuilder password, char a, char b)
    {
        for (var i = 0; i < password.Length; i++)
        {
            if (password[i] == a)
                password[i] = b;
            else if (password[i] == b)
                password[i] = a;
        }

        return password;
    }

    private static StringBuilder Rotate(StringBuilder password, int x) => password.Shift(x);

    private static StringBuilder RotateFromLetter(StringBuilder password, char x)
    {
        var index = password.IndexOf(x);
        return Rotate(password, (index + (index < 4 ? 1 : 2)));
    }

    private static StringBuilder Reverse(StringBuilder password, int a, int b)
    {
        var (x, y) = a < b ? (a, b) : (b, a);

        while (x < y)
        {
            password = SwapPositions(password, x, y);
            x++;
            y--;
        }

        return password;
    }

    private static StringBuilder Move(StringBuilder password, int a, int b)
    {
        char x = password[a];
        password = password.Remove(a, 1);
        return password.Insert(b, x);
    }

    private static StringBuilder RotateFromLetterReversed(StringBuilder password, char x)
    {
        var index = password.IndexOf(x);
        return Rotate(password, -index + Enumerable.Range(0, 8).First(i => (2 * i + (i < 4 ? 1 : 2)).Mod(password.Length) == index));
    }
}

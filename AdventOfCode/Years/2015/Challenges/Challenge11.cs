using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Text;

namespace AdventOfCode2015.Challenges;

[Challenge(11)]
public class Challenge11
{
    [Part(1, "hepxxyzz")]
    public string Part1(string input)
    {
        string password = input;
        do
        {
            password = Increment(password);
        } while (!Validate(password));

        return password;
    }

    [Part(2, "heqaabcc")]
    public string Part2(string input)
    {
        string password = Part1(input);
        do
        {
            password = Increment(password);
        } while (!Validate(password));

        return password;
    }

    public static string Increment(string password)
    {
        var sb = new StringBuilder();
        var stack = new Stack<char>();
        for (var i = password.Length - 1; i >= 0; i--)
        {
            var newChar = (char)('a' + (password[i] - 'a' + 1) % 26);
            stack.Push(newChar);
            if (newChar == 'a')
                continue;

            sb.Append(password[..i]);
            break;
        }

        while (stack.Count > 0)
            sb.Append(stack.Pop());

        return sb.ToString();
    }

    public static bool Validate(string password) =>
        Validate3AscendingCharacterRule(password) &&
        ValidateNoIOLLettersRule(password) &&
        ValidateAtLeast2DifferentPairsRule(password);

    public static bool Validate3AscendingCharacterRule(string password)
        => password.Windowed(3).Any(w => w[2] - w[1] == 1 && w[1] - w[0] == 1);

    public static bool ValidateNoIOLLettersRule(string password)
        => password.ContainsAnyExcept(['i', 'o', 'l']);

    public static bool ValidateAtLeast2DifferentPairsRule(string password)
    {
        var firstPair = password
            .Windowed(2, (w, i) => (Item: w, Index: i))
            .FirstOrDefault(w => w[0].Item == w[1].Item);

        if (firstPair is null)
            return false; // no pairs

        var lastPair = password
            .Windowed(2, (w, i) => (Item: w, Index: i))
            .LastOrDefault(w => w[0].Item == w[1].Item && w[0].Item != firstPair[0].Item && w[1].Item != firstPair[1].Item && w[0].Index > firstPair[1].Index);

        return lastPair is { };
    }
}

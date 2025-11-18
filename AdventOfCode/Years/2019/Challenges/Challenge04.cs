using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(4)]
public class Challenge04
{
    const int Min = 153517;
    const int Max = 630395;

    [Part(1, "1729")]
    public string Part1(string input) => Enumerable.Range(Min, Max - Min)
        .Select(password => password.ToString())
        .Count(password => MatchAdjacent(password) && NeverDecrease(password))
        .ToString();

    [Part(2, "1172")]
    public string Part2(string input) => Enumerable.Range(Min, Max - Min)
        .Select(password => password.ToString())
        .Count(password => MatchAdjacentPairButNotThree(password) && NeverDecrease(password))
        .ToString();

    // Two adjacent digits are the same (like 22 in 122345).
    private static bool MatchAdjacent(string password)
    {
        foreach (var window in password.Windowed(2))
            if (window[0] == window[1])
                return true;

        return false;
    }

    // Two adjacent digits are the same but not 3 (like 22 in 122345).
    private static bool MatchAdjacentPairButNotThree(string password)
    {
        if (password[0] == password[1] && password[1] != password[2])
            return true;

        if (password[^1] == password[^2] && password[^2] != password[^3])
            return true;

        for (var i = 1; i < password.Length - 2; i++)
        {
            if (password[i] == password[i + 1] && password[i] != password[i - 1] && password[i + 2] != password[i + 1])
                return true;
        }

        return false;
    }

    // Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
    private static bool NeverDecrease(string password)
    {
        foreach (var window in password.Windowed(2))
        {
            if (char.GetNumericValue(window[0]) > char.GetNumericValue(window[1]))
                return false;
        }

        return true;
    }
}

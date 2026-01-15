using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(5)]
public class Challenge05
{
    [Part(1, "10132")]
    public string Part1(string input) => Reduce(input).Count.ToString();

    [Part(2, "4572")]
    public string Part2(string input) => "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        .Min(c => Reduce(input.Where(x => char.ToUpperInvariant(x) != c).AsString()).Count)
        .ToString();

    private static IReadOnlyCollection<char> Reduce(string input)
    {
        var stack = new Stack<char>();
        foreach (var c in input)
        {
            if (stack.Count == 0)
            {
                stack.Push(c);
                continue;
            }

            if (IsPair(stack.Peek(), c))
                stack.Pop();
            else
                stack.Push(c);
        }

        return stack;
    }

    private static bool IsPair(char a, char b)
    {
        var isAUpper = char.IsUpper(a);
        var isBUpper = char.IsUpper(b);

        if (!(isAUpper ^ isBUpper))
            return false;

        return char.ToUpperInvariant(a) == char.ToUpperInvariant(b);
    }
}

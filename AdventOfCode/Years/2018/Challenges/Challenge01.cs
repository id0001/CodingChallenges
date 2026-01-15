using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(1)]
public class Challenge01
{
    [Part(1, "595")]
    public string Part1(string input) => input.Lines(line => line.As<int>()).Sum().ToString();

    [Part(2, "80598")]
    public string Part2(string input)
    {
        var visited = new HashSet<int>([0]);
        var f = 0;
        foreach(var change in input.Lines(line => line.As<int>()).Cycle())
        {
            f += change;
            if (!visited.Add(f))
                return f.ToString();
        }

        throw new InvalidOperationException();
    }
}

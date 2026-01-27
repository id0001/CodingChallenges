using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Collections.Immutable;
using System.Text;

namespace AdventOfCode2018.Challenges;

[Challenge(12)]
public class Challenge12
{
    [Part(1, "3405")]
    public string Part1(string input)
    {
        var (state, groups) = ParseInput(input);
        int min = 0, max = 0;
        for (var i = 0; i < 20; i++)
        {
            var newState = new HashSet<int>();
            (min, max) = (state.Min(), state.Max());
            for (var x = min - 2; x <= max + 2; x++)
            {
                var key = GetKey(state, x);
                if (groups.TryGetValue(key, out char value) && value == '#')
                    newState.Add(x);
            }

            state = newState;
        }

        (min, max) = (state.Min(), state.Max());
        return Enumerable.Range(min, max - min + 1).Where(state.Contains).Sum().ToString();
    }

    [Part(2, "3350000000000")]
    public string Part2(string input)
    {
        var (state, groups) = ParseInput(input);
        int min = 0, max = 0;
        for (long i = 0; ; i++)
        {
            var newState = new HashSet<int>();
            (min, max) = (state.Min(), state.Max());
            for (var x = min - 2; x <= max + 2; x++)
            {
                var key = GetKey(state, x);
                if (groups.TryGetValue(key, out char value) && value == '#')
                    newState.Add(x);
            }

            if (StateEqual(state, newState))
            {
                var add = 50_000_000_000 - i;
                (min, max) = (state.Min(), state.Max());
                return Enumerable.Range(min, max - min + 1).Where(state.Contains).Select(ix => ix + add).Sum().ToString();
            }

            state = newState;
        }
    }

    private static bool StateEqual(HashSet<int> a, HashSet<int> b)
    {
        if(a.Count != b.Count) 
            return false;

        var (amin,amax) = (a.Min(), a.Max());
        var (bmin,bmax) = (b.Min(), b.Max());

        if (amax - amin != bmax - bmin)
            return false;

        for(var i = 0; i < amax-amin; i++)
        {
            if (a.Contains(amin + i) != b.Contains(bmin + i))
                return false;
        }

        return true;
    }

    private static string GetKey(HashSet<int> set, int index)
    {
        var sb = new StringBuilder();
        for (var i = index - 2; i <= index + 2; i++)
            sb.Append(set.Contains(i) ? '#' : '.');

        return sb.ToString();
    }

    private static (HashSet<int>, Dictionary<string, char>) ParseInput(string input)
        => input.Paragraphs().Transform(x => (parseInitialState(x.First()), ParseConversionGroups(x.Second())));

    private static HashSet<int> parseInitialState(string input)
    {
        var state = input.Extract<string>(@"initial state: (.+)");
        var set = new HashSet<int>();
        for (var i = 0; i < state.Length; i++)
        {
            if (state[i] == '#')
                set.Add(i);
        }

        return set;
    }

    private static Dictionary<string, char> ParseConversionGroups(string input)
        => input.Lines(line => line.SplitBy<string, char>("=>")).ToDictionary(kv => kv.First, kv => kv.Second);
}

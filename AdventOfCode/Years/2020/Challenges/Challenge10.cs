using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2020.Challenges;

[Challenge(10)]
public class Challenge10
{
    [Part(1, "2760")]
    public string Part1(string input)
    {
        var adapters = input.Lines().As<int>().OrderBy(x => x).ToList();
        var differences = new[] { 0, 0, 1 }; // Start with a difference of 3 for the device's built-in adapter
        var prev = 0;
        foreach (var adapter in adapters)
        {
            differences[adapter - prev - 1]++;
            prev = adapter;
        }

        return (differences[0] * differences[2]).ToString();
    }

    [Part(2, "13816758796288")]
    public string Part2(string input)
    {
        var adapters = input.Lines().As<int>().OrderBy(x => x).ToList();

        var max = adapters[^1] + 3;
        var set = adapters.Prepend(0).Append(max).ToArray();
        var groupCount = new[] { 0, 0, 0 };

        var c = 0;
        for(var i = 0; i < set.Length; i++)
        {
            if(i == 0 || i == set.Length -1 || set[i] - set[i-1] == 3 || set[i+1] - set[i-1] > 3)
            {
                if (c > 0)
                    groupCount[c - 1]++;

                c = 0;
                continue;
            }

            c++;
        }

        if (c > 0)
            groupCount[c - 1]++;

        var count = (long)Math.Pow(2, groupCount[0]) * (long)Math.Pow(4, groupCount[1]) * (long)Math.Pow(7, groupCount[2]);
        return count.ToString();
    }
}

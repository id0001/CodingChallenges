using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Immutable;
using CodingChallenge.Utilities.Extensions;
using System.Linq;

namespace AdventOfCode2017.Challenges;

[Challenge(6)]
public class Challenge06
{
    [Part(1, "12841")]
    public string Part1(string input)
    {
        var banks = input.SplitBy<int>("\t").ToArray();

        var visited = new HashSet<ImmutableValueArray<int>> { new(banks) };
        var cycle = 0;

        while (true)
        {
            var (i, blocks) = banks.Index().MaxBy(x => x.Item);
            banks[i] = 0;

            while (blocks > 0)
            {
                banks[(i + 1).Mod(16)]++;
                blocks--;
                i++;
            }

            cycle++;

            var arr = new ImmutableValueArray<int>(banks);
            if (visited.Contains(arr))
                return cycle.ToString();

            visited.Add(arr);
        }
    }

    [Part(2, "8038")]
    public string Part2(string input)
    {
        var banks = input.SplitBy<int>("\t").ToArray();

        var visited = new Dictionary<ImmutableValueArray<int>, int>() { { new(banks), 0 } };

        var cycle = 0;

        while (true)
        {
            var (i, blocks) = banks.Index().MaxBy(x => x.Item);
            banks[i] = 0;

            while (blocks > 0)
            {
                banks[(i + 1).Mod(16)]++;
                blocks--;
                i++;
            }

            cycle++;

            var arr = new ImmutableValueArray<int>(banks);
            if (visited.ContainsKey(arr))
                return (cycle - visited[arr]).ToString();

            visited.Add(arr, cycle);
        }
    }
}

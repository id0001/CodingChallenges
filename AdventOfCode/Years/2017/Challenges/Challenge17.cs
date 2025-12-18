using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(17)]
public class Challenge17
{
    [Part(1, "1173")]
    public string Part1(string input)
    {
        var intInput = input.As<int>();

        var list = new LinkedList<int>();
        list.AddLast(0);

        var current = list.First!;
        for(var i = 0; i < 2017; i++)
        {
            var steps = intInput % list.Count;
            for (var j = 0; j < steps; j++)
                current = current.NextOrFirst();

            list.AddAfter(current, i + 1);
            current = current.NextOrFirst();
        }

        return list.Find(2017)!.NextOrFirst().Value.ToString();
    }

    [Part(2, "1930815")]
    public string Part2(string input)
    {
        var intInput = input.As<int>();

        var count = 1;
        var currentIndex = 0;
        var result = 0;

        for (var i = 0; i < 50_000_000; i++)
        {
            currentIndex = (currentIndex + intInput).Mod(count) + 1;
            if (currentIndex == 1) // this currently is the answer
                result = i + 1;

            count++;
        }

        return result.ToString();
    }
}

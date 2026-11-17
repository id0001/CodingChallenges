using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;
using Microsoft.Extensions.Options;

namespace AdventOfCode2020.Challenges;

[Challenge(9)]
public class Challenge09
{
    [Part(1, "1930745883")]
    public string Part1(string input)
    {
        var buffer = new CircularBuffer<long>(25);
        var index = 0;

        foreach(var value in input.Lines().As<long>())
        {
            if(index >= 25)
            {
                if (!Validate(buffer, value))
                    return value.ToString();
            }

            buffer.Enqueue(value);
            index++;
        }

        throw new InvalidOperationException("No solution found");
    }

    [Part(2, "268878261")]
    public string Part2(string input)
    {
        const long target = 1930745883L;

        var buffer = new CircularBuffer<long>(25);
        var index = 0;

        foreach (var value in input.Lines().As<long>())
        {
            if (index >= 25)
            {
                var set = FindContigiousSet(buffer, target);
                if (set.Length > 0)
                    return (set.Min() + set.Max()).ToString();
            }

            buffer.Enqueue(value);
            index++;
        }

        throw new InvalidOperationException("No solution found");
    }

    private static bool Validate(CircularBuffer<long> buffer, long value)
    {
        for(var y = 0; y < buffer.Count; y++)
        {
            for (var x = 0; x < buffer.Count; x++)
            {
                if (value == buffer[x] + buffer[y])
                    return true;
            }
        }

        return false;
    }

    private static long[] FindContigiousSet(CircularBuffer<long> buffer, long target)
    {
        var sum = buffer[0] + buffer[1];
        if(sum == target)
            return [.. buffer.Take(2)];

        for(var i = 2; i < buffer.Count; i++)
        {
            sum += buffer[i];
            if (sum == target)
                return [.. buffer.Take(i + 1)];

            if (sum > target)
                break;
        }

        return [];
    }
}

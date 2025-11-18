using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Challenges;

[Challenge(14)]
public class Challenge14
{
    [Part(1, "23769")]
    public string Part1(string input)
    {
        using var md5 = MD5.Create();

        CircularBuffer<string> buffer = new CircularBuffer<string>(1001);

        var keysFound = 0;
        for(var i = 0; ; i++)
        {
            buffer.Enqueue(GetHash(md5, input, i, 0));

            if(i >= 1001)
            {
                if (ContainsTriplet(buffer[0], out var symbol) && buffer.Skip(1).Any(hash => ContainsQuintuplet(hash!, symbol)))
                {
                    keysFound++;
                    if (keysFound == 64)
                        return (i - 1000).ToString();
                }
            }
        }
    }

    [Part(2, "20606")]
    public string Part2(string input)
    {
        using var md5 = MD5.Create();

        CircularBuffer<string> buffer = new CircularBuffer<string>(1001);

        var keysFound = 0;
        for (var i = 0; ; i++)
        {
            buffer.Enqueue(GetHash(md5, input, i, 2016));

            if (i >= 1001)
            {
                if (ContainsTriplet(buffer[0], out var symbol) && buffer.Skip(1).Any(hash => ContainsQuintuplet(hash!, symbol)))
                {
                    keysFound++;
                    if (keysFound == 64)
                        return (i - 1000).ToString();
                }
            }
        }
    }

    private bool ContainsTriplet(string input, out char symbol)
    {
        symbol = '!';

        foreach (var window in input.Windowed(3))
        {
            if (window[0] == window[1] && window[1] == window[2])
            {
                symbol = window[0];
                return true;
            }
        }

        return false;
    }

    private bool ContainsQuintuplet(string input, char symbol)
    {
        foreach (var window in input.Windowed(5))
            if (window.All(c => c == symbol))
                return true;

        return false;
    }

    private static string GetHash(MD5 md5, string input, int index, int stretch)
    {
        var hash = Convert.ToHexString(md5.ComputeHash(Encoding.Default.GetBytes(input + index))).ToLowerInvariant();

        for (var i = 0; i < stretch; i++)
            hash = Convert.ToHexString(md5.ComputeHash(Encoding.Default.GetBytes(hash))).ToLowerInvariant();

        return hash;
    }
}

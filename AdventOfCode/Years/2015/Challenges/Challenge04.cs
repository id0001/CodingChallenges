using CodingChallenge.Utilities.Attributes;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2015.Challenges;

[Challenge(4)]
public class Challenge04
{
    [Part(1, "117946")]
    public string Part1(string input) => GetIndexOfHashStartingWith(input, (output => output[0] == 0 && output[1] == 0 && output[2] < 16)).ToString();

    [Part(2, "3938038")]
    public string Part2(string input) => GetIndexOfHashStartingWith(input, (output => output[0] == 0 && output[1] == 0 && output[2] == 0)).ToString();

    public static int GetIndexOfHashStartingWith(string password, Func<ReadOnlySpan<byte>, bool> check)
    {
        Span<byte> hash = stackalloc byte[16];

        for (var i = 0; ; i++)
        {
            var input = Encoding.ASCII.GetBytes($"{password}{i}");
            MD5.HashData(input, hash);
            if (check(hash))
                return i;
        }
    }
}
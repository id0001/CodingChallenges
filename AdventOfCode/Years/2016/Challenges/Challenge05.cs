using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Challenges;

[Challenge(5)]
public class Challenge05
{
    [Part(1, "801b56a7")]
    public string Part1(string input)
    {
        using var md5 = MD5.Create();

        StringBuilder pass = new();
        var index = 0;
        while (pass.Length < 8)
        {
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(input + index));
            if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
                pass.Append((hash[2] & 0x0F).ToHexString(1, true));

            index++;
        }

        return pass.ToString().ToLowerInvariant();
    }

    [Part(2, "424a0197")]
    public string Part2(string input)
    {
        using var md5 = MD5.Create();

        var pass = new char[8];
        var charsFound = 0;
        var index = 0;
        while (charsFound < 8)
        {
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(input + index));
            if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
                if (int.TryParse((hash[2] & 0x0F).ToHexString(1, true), out var i) && i <= 7 && pass[i] == default)
                {
                    pass[i] = (hash[3] & 0xF0).ToHexString(1, true)[0];
                    charsFound++;
                }

            index++;
        }

        return string.Join(string.Empty, pass).ToLowerInvariant();
    }
}

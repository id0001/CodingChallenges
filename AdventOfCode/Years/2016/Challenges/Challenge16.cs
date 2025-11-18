using CodingChallenge.Utilities.Attributes;
using System.Text;

namespace AdventOfCode2016.Challenges;

[Challenge(16)]
public class Challenge16
{
    [Part(1, "10100011010101011")]
    public string Part1(string input)
    {
        var length = 272;
        var sb = new StringBuilder(input, length * 2);

        while (sb.Length < length)
            CreateDragonCurve(sb);

        sb.Length = length; // truncate
        return CreateChecksum(sb);
    }

    [Part(2, "01010001101011001")]
    public string Part2(string input)
    {
        var length = 35651584;
        var sb = new StringBuilder(input, length * 2);

        while (sb.Length < length)
            CreateDragonCurve(sb);

        sb.Length = length; // truncate
        return CreateChecksum(sb);
    }

    private static void CreateDragonCurve(StringBuilder input)
    {
        var len = input.Length;
        input.Append('0');
        for (var i = len - 1; i >= 0; i--)
            input.Append(input[i] == '0' ? '1' : '0');
    }

    private static string CreateChecksum(StringBuilder input)
    {
        do
        {
            var len = input.Length;
            for (var i = 1; i < len; i += 2)
                input.Append(input[i - 1] == input[i] ? '1' : '0');

            input.Remove(0, len);
        } while (input.Length % 2 == 0);

        return input.ToString();
    }
}

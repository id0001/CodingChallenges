using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(16)]
public class Challenge16
{
    private static readonly int[] Pattern = [0, 1, 0, -1];

    [Part(1, "68317988")]
    public string Part1(string input)
    {
        var seq = input.Select(x => (int)char.GetNumericValue(x)).ToArray();

        for (var phase = 0; phase < 100; phase++)
            for (var indexOut = 0; indexOut < seq.Length; indexOut++)
                seq[indexOut] = Math.Abs(seq.Index().Sum(item => item.Item * GetPatternValueAtIndex(indexOut, item.Index))) % 10;

        return string.Join("", seq.Take(8));
    }

    [Part(2, "53850800")]
    public string Part2(string input)
    {
        var seq = input.Select(x => (int)char.GetNumericValue(x)).ToArray();

        var seqCount = seq.Length * 10000;
        var offset = string.Join("", seq.Take(7)).As<int>();

        var newInput = new int[seqCount - offset];
        for (var i = 0; i < newInput.Length; i++)
            newInput[i] = GetInput(seq, offset + i);

        for (var phase = 0; phase < 100; phase++)
        {
            var sum = 0;
            for (var indexOut = newInput.Length-1; indexOut >= 0; indexOut--)
            {
                sum += newInput[indexOut];
                newInput[indexOut] = sum % 10;
            }
        }

        return string.Join("", newInput.Take(8));
    }

    private static int GetInput(int[] sequence, int offset) => sequence[offset % sequence.Length];

    private static int GetPatternValueAtIndex(int outputIndex, int inputIndex)
    {
        var patternSize = (outputIndex + 1) * 4;
        var pi = (inputIndex + 1) % patternSize;
        var bpi = (int)Math.Floor(pi / (outputIndex + 1d));
        return Pattern[bpi];
    }
}

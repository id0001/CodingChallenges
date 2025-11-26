using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Mathematics;
using System.Linq.Expressions;
using System.Numerics;

namespace AdventOfCode2019.Challenges;

[Challenge(22)]
public class Challenge22
{
    [Part(1, "5472")]
    public string Part1(string input)
    {
        var shuffles = input.Lines(ParseLine).ToList();
        var position = 2019;
        var deckSize = 10007;

        foreach (var shuffle in shuffles)
        {
            position = shuffle.Type switch
            {
                Technique.Reverse => Reverse(position, deckSize),
                Technique.Increment => Increment(position, deckSize, shuffle.Amount),
                Technique.Cut => Cut(position, deckSize, shuffle.Amount),
                _ => throw new NotImplementedException(),
            };
        }

        return position.ToString();
    }



    [Part(2, "64586600795606")]
    public string Part2(string input)
    {
        var shuffles = input.Lines(ParseLine).ToList();
        var deckSize = new BigInteger(119315717514047L); // N
        var times = new BigInteger(101741582076661L); // M
        BigInteger target = 2020;

        // Combine all the shuffle operations into 1 formula.
        BigInteger a = BigInteger.One, b = BigInteger.Zero;
        foreach (var shuffle in shuffles)
        {
            var (la, lb) = shuffle.Type switch
            {
                Technique.Reverse => (BigInteger.MinusOne, BigInteger.MinusOne), // Reverse(-x-1 % N) -> a = -1, b = -1
                Technique.Increment => (new BigInteger(shuffle.Amount), BigInteger.Zero), // Increment(xn % N) -> a = n, b = 0
                Technique.Cut => (BigInteger.One, new BigInteger(-shuffle.Amount)), // Cut(x-n % N) -> a = 1, b = -n
                _ => throw new NotImplementedException()
            };

            a = (la * a).Mod(deckSize);
            b = (la * b + lb).Mod(deckSize);
        }

        // This operation now needs to be applied M times.
        // la = a, lb = b
        // a = 1, b = 0
        // for(range(0, M)):
        //    (a = (a * la) % N, b = (la * b + lb) % N)

        // a => (a^M) % N
        // b => (a^(M-1)b)+(a^(M-2)b)+...+(a^(M-M)b) => b * ((a^(M-1))+(a^(M-2))+...+(a^(M-M))) => ((b * ((a^M)-1)) / (a-1)) % N

        var ma = BigInteger.ModPow(a, times, deckSize); // ma = a^M % N
        var mb = (b * (ma - 1) * NumberTheory.ModInverse(a - 1, deckSize)).Mod(deckSize); // mb = ((b * ((a^M)-1) / (a-1)) % N
        var result = ((target - mb) * NumberTheory.ModInverse(ma, deckSize)).Mod(deckSize); // (x-b) / a % N
        return result.ToString();
    }

    private static Shuffle ParseLine(string line)
    {
        if (line == "deal into new stack")
            return new Shuffle(Technique.Reverse, 0);

        if (line.TryExtract<int>(@"cut (-?\d+)", out var amount))
            return new Shuffle(Technique.Cut, amount);

        if (line.TryExtract<int>(@"deal with increment (-?\d+)", out amount))
            return new Shuffle(Technique.Increment, amount);

        throw new NotImplementedException();
    }

    private static int Cut(int position, int deckSize, int amount) => (position - amount).Mod(deckSize);

    private static int Increment(int position, int deckSize, int amount) => (position * amount).Mod(deckSize);

    private static int Reverse(int position, int deckSize) => (-position - 1).Mod(deckSize);

    private record Shuffle(Technique Type, int Amount);
    private enum Technique
    {
        Reverse,
        Increment,
        Cut
    }
}

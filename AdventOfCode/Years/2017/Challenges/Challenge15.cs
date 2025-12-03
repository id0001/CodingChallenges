using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(15)]
public class Challenge15
{
    private const long Divisor = 2147483647;
    private const long FactorA = 16807;
    private const long FactorB = 48271;


    [Part(1, "650")]
    public string Part1(string input)
    {
        var (a, b) = input.Lines(line => line.Extract<long>(@"Generator \w starts with (\d+)")).Transform(x => (x.First(), x.Second()));

        return Enumerable
            .Unfold(new State(a, b, 0), x => new State(NextValue(x.A, FactorA), NextValue(x.B, FactorB), x.Iterations + 1))
            .TakeWhile(x => x.Iterations < 40_000_000)
            .Where(x => x.IsMatch)
            .Count()
            .ToString();
    }

    [Part(2, "336")]
    public string Part2(string input)
    {
        var (a, b) = input.Lines(line => line.Extract<long>(@"Generator \w starts with (\d+)")).Transform(x => (x.First(), x.Second()));

        return Enumerable
            .Unfold(new State(a, b, 0), x => new State(NextValueDivisableBy(x.A, FactorA, 4), NextValueDivisableBy(x.B, FactorB, 8), x.Iterations + 1))
            .TakeWhile(x => x.Iterations < 5_000_000)
            .Where(x => x.IsMatch)
            .Count()
            .ToString();
    }

    private static bool IsMatching(long a, long b) => (a & 0xffff) == (b & 0xffff);

    private static long NextValue(long v, long factor) => (v * factor) % Divisor;

    private static long NextValueDivisableBy(long value, long factor, long divisor)
        => Enumerable.Unfold(value, v => NextValue(v, factor), true).First(v => v % divisor == 0);

    private record State(long A, long B, int Iterations)
    {
        public bool IsMatch => IsMatching(A, B);
    }
}

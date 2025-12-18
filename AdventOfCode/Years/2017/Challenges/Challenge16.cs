using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(16)]
public class Challenge16
{
    [Part(1, "kbednhopmfcjilag")]
    public string Part1(string input)
    {
        int[] programs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];

        foreach (var move in input.SplitBy(","))
        {
            switch (move[0])
            {
                case 's':
                    Spin(programs, move[1..]);
                    break;
                case 'x':
                    Exchange(programs, move[1..]);
                    break;
                case 'p':
                    Partner(programs, move[1..]);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        return Glue(programs);
    }

    [Part(2, "fbmcgdnjakpioelh")]
    public string Part2(string input)
    {
        int[] programs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
        var moves = input.SplitBy(",");

        for (var i = 0; i < 1_000_000_000; i++)
        {
            foreach (var move in moves)
            {
                switch (move[0])
                {
                    case 's':
                        Spin(programs, move[1..]);
                        break;
                    case 'x':
                        Exchange(programs, move[1..]);
                        break;
                    case 'p':
                        Partner(programs, move[1..]);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            if (Glue(programs) == "abcdefghijklmnop")
                i += (1_000_000_000 / (i + 1) - 1) * (i + 1); // i + (cycles * length)
        }

        return Glue(programs);
    }

    private static void Spin(int[] programs, string move)
    {
        var x = move.As<int>();
        for (var i = 0; i < programs.Length; i++)
            programs[i] = (programs[i] + x).Mod(programs.Length);
    }

    private static void Exchange(int[] programs, string move)
    {
        var (a, b, _) = move.SplitBy<int>("/").Select(i => Array.IndexOf(programs, i));
        (programs[a], programs[b]) = (programs[b], programs[a]);
    }

    private static void Partner(int[] programs, string move)
    {
        var (a, b, _) = move.SplitBy<char>("/").Select(GetIndex);
        (programs[a], programs[b]) = (programs[b], programs[a]);
    }

    private static string Glue(int[] programs)
        => string.Join(string.Empty, programs.Zip([.. "abcdefghijklmnop"]).OrderBy(x => x.First).Select(x => x.Second));

    private static int GetIndex(char c) => c - 'a';
}

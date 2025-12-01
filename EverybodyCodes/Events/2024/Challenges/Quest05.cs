using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using Spectre.Console;
using System.Collections.Generic;

namespace EverybodyCodes2024.Challenges;

[Challenge(5)]
public class Quest05
{
    [Part(1, "3222")]
    public string Part1(string input)
    {
        var grid = input.Lines(line => line.SplitBy<int>(" ").ToArray()).ToArray().To2dArray();
        var dancers = new Dancers(grid);

        for (var i = 0; i < 10; i++)
            dancers.MoveNext();

        return dancers.Shout();
    }

    [Part(2, "12881059969561")]
    public string Part2(string input)
    {
        var grid = input.Lines(line => line.SplitBy<int>(" ").ToArray()).ToArray().To2dArray();
        var dancers = new Dancers(grid);

        var dict = new Dictionary<string, int>();

        for (var r = 1; ; r++)
        {
            dancers.MoveNext();
            var number = dancers.Shout();
            if (!dict.TryAdd(number, 1))
                dict[number]++;

            if (dict[number] == 2024)
                return (number.As<long>() * r).ToString();
        }
    }

    [Part(3, "9107100310021002")]
    public string Part3(string input)
    {
        var grid = input.Lines(line => line.SplitBy<int>(" ").ToArray()).ToArray().To2dArray();

        var dancers = new Dancers(grid);

        long max = 0;
        for (var r = 1; r < 1000 ; r++)
        {
            dancers.MoveNext();
            var number = dancers.Shout().As<long>();
            max = Math.Max(max, number);
        }

        return max.ToString();
    }

    private static int GetInsertIndex(int amount, int length)
    {
        bool forward = true;
        int index = 0;
        for (var n = 1; n < amount; n++)
        {
            index += forward ? 1 : -1;

            if (index < 0)
            {
                index = 0;
                forward = true;
            }
            else if (index >= length)
            {
                index = length - 1;
                forward = false;
            }
        }

        return forward ? index : index + 1;
    }

    private sealed class Dancers
    {
        private int _col = 0;
        private readonly List<int>[] _columns = [[], [], [], []];

        public Dancers(int[,] input)
        {
            for (var c = 0; c < 4; c++)
                foreach (var dancer in input.Column(c))
                    _columns[c].Add(dancer);
        }

        public void MoveNext()
        {
            var d = _columns[_col][0];

            _columns[_col].RemoveAt(0);
            var _nextCol = (_col + 1).Mod(4);

            var ins = GetInsertIndex(d, _columns[_nextCol].Count);
            _columns[_nextCol].Insert(ins, d);
            _col = _nextCol;
        }

        public string Shout() => string.Join(string.Empty, _columns[0][0].ToString(), _columns[1][0].ToString(), _columns[2][0].ToString(), _columns[3][0].ToString());
    }
}

using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using Spectre.Console;

namespace EverybodyCodes2024.Challenges;

[Challenge(5)]
public class Quest05
{
    [Part(1)]
    public string Part1(string input)
    {
        input = "2 3 4 5\r\n3 4 5 2\r\n4 5 2 3\r\n5 2 3 4";

        var grid = input.Replace(" ", string.Empty).ToGrid((_, c) => c.AsInteger);

        var square = new Square(grid);

        var dancer = new Dancer(grid[0, 0], Point2.Zero);
        for (var r = 0; r < 10; r++)
        {
            dancer = square.Progress(dancer);

            AnsiConsole.WriteLine(square.Shout());
        }

        return square.Shout();
    }

    //[Part(2)]
    //public string Part2(string input)
    //{
    //}

    //[Part(3)]
    //public string Part3(string input)
    //{
    //}

    private record Dancer(int Number, Point2 InitialPosition);

    private class Square
    {
        public readonly List<Dancer>[] _columns;

        public Square(Grid2<int> grid)
        {
            _columns = new List<Dancer>[4];
            for (var x = 0; x < grid.Columns; x++)
            {
                _columns[x] = new List<Dancer>();
                for (var y = 0; y < grid.Rows; y++)
                {
                    _columns[x].Add(new Dancer(grid[y, x], new Point2(x, y)));
                }
            }
        }

        public Dancer Progress(Dancer d)
        {
            for (var c = 0; c < _columns.Length; c++)
            {
                if (_columns[c].Contains(d))
                {
                    _columns[c].Remove(d);

                    c = (c + 1) % 4;
                    var i = d.Number % _columns[c].Count;
                    _columns[c].Insert(i, d);

                    return _columns[c][0];
                }
            }

            throw new InvalidOperationException();
        }

        public string Shout() => string.Join(string.Empty, [_columns[0][0].Number, _columns[1][1].Number, _columns[2][0].Number, _columns[3][0].Number]);
    }
}

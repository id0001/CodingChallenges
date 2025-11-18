using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(2)]
public class Challenge02
{
    [Part(1, "99332")]
    public string Part1(string input)
    {
        var keypad = new[,]
        {
            {'1', '2', '3'},
            {'3', '4', '5'},
            {'7', '8', '9'}
        };

        return string.Join(string.Empty, input.Lines(x => GetNumber(keypad, x)));
    }

    [Part(2, "DD483")]
    public string Part2(string input)
    {
        var keypad = new[,]
        {
            {'.', '.', '1', '.', '.'},
            {'.', '2', '3', '4', '.'},
            {'5', '6', '7', '8', '9'},
            {'.', 'A', 'B', 'C', '.'},
            {'.', '.', 'D', '.', '.'}
        };

        return string.Join(string.Empty, input.Lines(x => GetNumber(keypad, x)));
    }

    private static char GetNumber(char[,] keypad, IEnumerable<char> code)
    {
        var bounds = keypad.Bounds;

        return code.Aggregate(new Point2(1, 1), (pos, dir) => dir switch
        {
            'U' when bounds.Contains(pos.Up) && keypad[pos.Up.Y, pos.Up.X] != '.' => pos.Up,
            'R' when bounds.Contains(pos.Right) && keypad[pos.Right.Y, pos.Right.X] != '.' => pos.Right,
            'D' when bounds.Contains(pos.Down) && keypad[pos.Down.Y, pos.Down.X] != '.' => pos.Down,
            'L' when bounds.Contains(pos.Left) && keypad[pos.Left.Y, pos.Left.X] != '.' => pos.Left,
            _ => pos
        }).Transform(p => keypad[p.Y,p.X]);
    }
}

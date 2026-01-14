using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(22)]
public class Challenge22
{
    [Part(1, "5246")]
    public string Part1(string input)
    {
        var grid = input.ToSpatialMap('.');
        var current = new Pose2(new Point2(grid.Bounds.Width / 2, grid.Bounds.Height / 2), Face.Up);

        int infections = 0;
        for(var i = 0; i < 10_000; i++)
        {
            current = grid[current.Position] switch
            {
                '#' => current.TurnRight(),
                _ => current.TurnLeft()
            };

            grid[current.Position] = grid[current.Position] == '#' ? '.' : '#';
            if (grid[current.Position] == '#')
                infections++;

            current = current.Step();
        }

        return infections.ToString();
    }

    [Part(2, "2512059")]
    public string Part2(string input)
    {
        var grid = input.ToSpatialMap('.');
        var current = new Pose2(new Point2(grid.Bounds.Width / 2, grid.Bounds.Height / 2), Face.Up);

        int infections = 0;
        for (var i = 0; i < 10_000_000; i++)
        {
            current = grid[current.Position] switch
            {
                '#' => current.TurnRight(),
                '.' => current.TurnLeft(),
                'F' => current.TurnLeft().TurnLeft(),
                _ => current
            };

            grid[current.Position] = grid[current.Position] switch
            {
                '#' => 'F',
                'F' => '.',
                '.' => 'W',
                'W' => '#',
                _ => throw new NotImplementedException()
            };

            if (grid[current.Position] == '#')
                infections++;

            current = current.Step();
        }

        return infections.ToString();
    }
}

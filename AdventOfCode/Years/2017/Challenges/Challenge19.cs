using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Text;

namespace AdventOfCode2017.Challenges;

[Challenge(19)]
public class Challenge19
{
    [Part(1, "QPRYCIOLU")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();
        var start = new Point2(Array.IndexOf(grid.Row(0).ToArray(), '|'), 0);
        var (_, result) = FollowToEnd(grid, start);

        return result;
    }

    [Part(2, "16162")]
    public string Part2(string input)
    {
        var grid = input.ToGrid();
        var start = new Point2(Array.IndexOf(grid.Row(0).ToArray(), '|'), 0);
        var (steps, _) = FollowToEnd(grid, start);

        return steps.ToString();
    }

    private static (int Steps, string Result) FollowToEnd(Grid2<char> grid, Point2 start)
    {
        var pose = new Pose2(start, Face.Down);
        var steps = 0;
        var result = new StringBuilder();

        while (grid.Bounds.Contains(pose.Position) && grid[pose.Position] != ' ')
        {
            switch (grid[pose.Position])
            {
                case '+':
                    pose = ChangeDirection(grid, pose);
                    break;
                case var c when c is not '|' and not '-':
                    result.Append(c);
                    break;
                default:
                    break;
            }

            pose = pose.Step();
            steps++;
        }

        return (steps, result.ToString());
    }

    private static Pose2 ChangeDirection(Grid2<char> grid, Pose2 pose)
        => grid.Bounds.Contains(pose.Left) && grid[pose.Left] != ' ' ? pose.TurnLeft() : pose.TurnRight();
}

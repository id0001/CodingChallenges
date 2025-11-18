using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;
using System.Text;

namespace AdventOfCode2019.Challenges;

[Challenge(11)]
public class Challenge11
{
    [Part(1, "2883")]
    public string Part1(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();

        var robot = new Pose2(Point2.Zero, Face.Up);
        var history = new Dictionary<Point2, long>();
        var action = 0;

        var cpu = new Cpu<long>(program);
        while (cpu.Next())
        {
            if (cpu.InputNeeded)
            {
                cpu.Write(history.GetValueOrDefault(robot.Position, 0));
            }

            if (cpu.HasOutput)
            {
                switch (action)
                {
                    case 0:
                        history[robot.Position] = cpu.Read();
                        action = 1;
                        break;
                    case 1:
                        robot = cpu.Read() == 0 ? robot.TurnLeft() : robot.TurnRight();
                        robot = robot.Step();
                        action = 0;
                        break;
                    default:
                        break;
                }
            }
        }

        return history.Count.ToString();
    }

    [Part(2, "LEPCPLGZ")]
    public string Part2(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();

        var robot = new Pose2(Point2.Zero, Face.Up);
        var history = new Dictionary<Point2, long>();
        history[robot.Position] = 1;
        var action = 0;

        var cpu = new Cpu<long>(program);
        while (cpu.Next())
        {
            if (cpu.InputNeeded)
            {
                cpu.Write(history.GetValueOrDefault(robot.Position, 0));
            }

            if (cpu.HasOutput)
            {
                switch (action)
                {
                    case 0:
                        history[robot.Position] = cpu.Read();
                        action = 1;
                        break;
                    case 1:
                        robot = cpu.Read() == 0 ? robot.TurnLeft() : robot.TurnRight();
                        robot = robot.Step();
                        action = 0;
                        break;
                    default:
                        break;
                }
            }
        }

        return DrawHull(history).Ocr();
    }

    private static bool[,] DrawHull(Dictionary<Point2, long> locations)
    {
        var keys = locations.Keys;

        var leftMost = keys.Min(e => e.X);
        var rightMost = keys.Max(e => e.X);
        var topMost = keys.Min(e => e.Y);
        var bottomMost = keys.Max(e => e.Y);

        var rows = bottomMost - topMost + 1;
        var cols = rightMost - leftMost + 1;

        var result = new bool[rows, cols];
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
                result[y, x] = locations.GetValueOrDefault(new Point2(x + leftMost, y + topMost), 0) == 1;
        }

        return result;
    }
}

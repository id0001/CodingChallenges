using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(3)]
public class Challenge03
{
    [Part(1, "273")]
    public string Part1(string input)
    {
        var wires = input.Lines(ParseLine).ToArray();
        return wires[0].Keys.Intersect(wires[1].Keys).Select(p => Point2.ManhattanDistance(Point2.Zero, p)).Min().ToString();
    }

    [Part(2, "15622")]
    public string Part2(string input)
    {
        var wires = input.Lines(ParseLine).ToArray();
        return wires[0].Keys.Intersect(wires[1].Keys).Select(p => wires[0][p] + wires[1][p]).Min().ToString();
    }

    private static Dictionary<Point2, int> ParseLine(string line)
    {
        var dict = new Dictionary<Point2, int>();
        var moves = line.SplitBy(",");

        Pose2 pose = new Pose2();
        int steps = 0;
        foreach (var move in moves)
        {
            pose = pose with
            {
                Face = move[0] switch
                {
                    'U' => Face.Up,
                    'R' => Face.Right,
                    'D' => Face.Down,
                    'L' => Face.Left,
                    _ => throw new NotImplementedException()
                }
            };

            for (var i = 0; i < move[1..].As<int>(); i++)
            {
                pose = pose.Step(1);
                steps++;
                dict.TryAdd(pose.Position, steps);
            }
        }

        return dict;
    }
}

using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(1)]
public class Challenge01
{
    [Part(1, "273")]
    public string Part1(string input) => Point2.ManhattanDistance(Point2.Zero, input
        .SplitBy(",")
        .Aggregate(new Pose2(Point2.Zero, Face.Up), (pose, line) => (line[0], line[1..].As<int>()) switch
        {
            ('R', var a) => pose.TurnRight().Step(a),
            ('L', var a) => pose.TurnLeft().Step(a),
            _ => throw new NotImplementedException()
        }).Position).ToString();

    [Part(2, "115")]
    public string Part2(string input)
    {
        HashSet<Point2> visited = [Point2.Zero];
        var pose = new Pose2(Point2.Zero, Face.Up);

        foreach(var (dir, amount) in input.SplitBy(",").Select(line => (line[0], line[1..].As<int>())))
        {
            pose = dir == 'R' ? pose.TurnRight() : pose.TurnLeft();
            for(var i = 0; i < amount; i++)
            {
                pose = pose.Step();
                if (!visited.Add(pose.Position))
                    return Point2.ManhattanDistance(Point2.Zero, pose.Position).ToString();
            }
        }

        throw new InvalidOperationException();
    }
}

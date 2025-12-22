using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(20)]
public class Challenge20
{
    [Part(1, "161")]
    public string Part1(string input)
    {
        var list = input.Lines(ParseLine).ToArray();

        for (var id = 0; id < list.Length; id++)
            list[id] = list[id].Update(1000);

        return list.Index().MinBy(x => Point3.ManhattanDistance(Point3.Zero, x.Item.Position)).Index.ToString();
    }

    [Part(2, "438")]
    public string Part2(string input)
    {
        var list = input.Lines(ParseLine).ToArray();

        for(var i = 0; i < 1000; i++)
        {
            list = list
                .Select(x => x.Update())
                .GroupBy(p => p.Position)
                .Where(g => g.Count() == 1)
                .SelectMany(g => g)
                .ToArray();
        }

        return list.Length.ToString();
    }

    private PosVelAcc ParseLine(string line) => line
        .Extract(@"p=<(.+)>, v=<(.+)>, a=<(.+)>")
        .Select(ToPoint)
        .Transform(x => new PosVelAcc(x.First(), x.Second(), x.Third()));


    private static Point3 ToPoint(string value) => value.SplitBy<int>(",").Transform(x => new Point3(x.First(), x.Second(), x.Third()));

    private record PosVelAcc(Point3 Position, Point3 Velocity, Point3 Acceleration)
    {
        public PosVelAcc Update()
        {
            var v = Velocity + Acceleration;
            var p = Position + v;
            return this with { Position = p, Velocity = v };
        }

        public PosVelAcc Update(int ticks)
        {
            var result = this;
            for(var i = 0; i < ticks; i++)
                result = result.Update();

            return result;
        }
    }
}

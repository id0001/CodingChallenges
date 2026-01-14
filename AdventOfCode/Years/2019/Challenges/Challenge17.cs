using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;
using System.Text;

namespace AdventOfCode2019.Challenges;

[Challenge(17)]
public class Challenge17
{
    [Part(1, "5680")]
    public string Part1(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();

        var spatialMap = new SpatialMap2<int>();

        var x = 0;
        var y = 0;

        var cpu = new Cpu<long>(program);
        while (cpu.Next())
        {
            if (cpu.HasOutput)
            {
                var o = (int)cpu.Read();
                if (o == 10)
                {
                    y++;
                    x = 0;
                    continue;
                }

                spatialMap[y, x] = o;
                x++;
            }
        }

        return spatialMap
            .Where(kv => kv.Value == 35 && IsIntersection(spatialMap, kv.Key))
            .Sum(kv => kv.Key.X * kv.Key.Y)
            .ToString();
    }

    [Part(2, "895965")]
    public string Part2(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();
        program[0] = 2;

        var spatialMap = new SpatialMap2<int>();

        string a = "R,12,R,4,R,10,R,12";
        string b = "R,6,L,8,R,10";
        string c = "L,8,R,4,R,4,R,6";
        string main = "A,B,A,C,A,B,C,A,B,C";

        var encoded = new[]
        {
            Encoding.ASCII.GetBytes(main),
            Encoding.ASCII.GetBytes(a),
            Encoding.ASCII.GetBytes(b),
            Encoding.ASCII.GetBytes(c),
            [110]
        };

        long dustCount = 0;

        var cpu = new Cpu<long>(program);
        var pos = Point2.Zero;
        while (cpu.Next())
        {
            if (cpu.InputNeeded)
            {
                if (pos.X >= encoded[pos.Y].Length)
                {
                    cpu.Write(10);
                    pos = pos with { X = 0, Y = pos.Y + 1 };
                    continue;
                }
             
                cpu.Write(encoded[pos.Y][pos.X]);
                pos = pos with { X = pos.X + 1 };
            }

            if (cpu.HasOutput)
                dustCount = cpu.Read();
        }

        return dustCount.ToString();
    }

    private static bool IsIntersection(SpatialMap2<int> map, Point2 p) => p.GetNeighbors().All(n => map[n] == 35);
}

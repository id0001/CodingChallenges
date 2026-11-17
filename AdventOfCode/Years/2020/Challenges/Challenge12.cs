using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Mathematics;

namespace AdventOfCode2020.Challenges;

[Challenge(12)]
public class Challenge12
{
    [Part(1, "1687")]
    public string Part1(string input)
    {
        var x = 0;
        var y = 0;
        var a = 0;
        foreach (var line in input.Lines())
        {
            var action = line[0];
            var value = int.Parse(line[1..]);

            switch (action)
            {
                case 'N': y -= value; break;
                case 'S': y += value; break;
                case 'E': x += value; break;
                case 'W': x -= value; break;
                case 'L': a = (a - value).Mod(360); break;
                case 'R': a = (a + value).Mod(360); break;
                case 'F':
                    var r = Trigonometry.DegreeToRadian(a);
                    x += (int)Math.Cos(r) * value;
                    y += (int)Math.Sin(r) * value;
                    break;
            }
        }

        return (Math.Abs(x) + Math.Abs(y)).ToString();
    }

    [Part(2, "20873")]
    public string Part2(string input)
    {
        var s = new Point2(0, 0);
        var w = new Point2(10, -1);

        foreach (var line in input.Lines())
        {
            var action = line[0];
            var value = int.Parse(line[1..]);

            switch (action)
            {
                case 'N': w = w with { Y = w.Y - value }; break;
                case 'S': w = w with { Y = w.Y + value }; break;
                case 'E': w = w with { X = w.X + value }; break;
                case 'W': w = w with { X = w.X - value }; break;
                case 'L': w = w.TurnAround(s, -Trigonometry.DegreeToRadian(value)); break;
                case 'R': w = w.TurnAround(s, Trigonometry.DegreeToRadian(value)); break;
                case 'F':
                    var r = Math.Atan2(w.Y - s.Y, w.X - s.X);
                    var dist = (w - s).ToVector2().Length;
                    var d = new Point2((int)Math.Round(Math.Cos(r) * dist), (int)Math.Round(Math.Sin(r) * dist));
                    s += d * value;
                    w += d * value;
                    break;
            }
        }

        return (Math.Abs(s.X) + Math.Abs(s.Y)).ToString();
    }
}

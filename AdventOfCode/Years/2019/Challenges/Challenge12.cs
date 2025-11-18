using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Mathematics;

namespace AdventOfCode2019.Challenges;

[Challenge(12)]
public class Challenge12
{
    [Part(1, "8310")]
    public string Part1(string input)
    {
        var moons = input.Lines(ParseLine).ToArray();

        for (var step = 0; step < 1000; step++)
        {
            foreach (var pair in moons.Combinations(2))
                Moon.ApplyGravity(pair[0], pair[1]);

            foreach (var moon in moons)
                moon.ApplyVelocity();
        }

        return moons.Sum(m => m.TotalEnergy).ToString();
    }

    [Part(2, "319290382980408")]
    public string Part2(string input)
    {
        var moons = input.Lines(ParseLine).ToArray();

        var steps = 0;
        var xCycle = 0;
        var yCycle = 0;
        var zCycle = 0;

        do
        {
            foreach (var pair in moons.Combinations(2))
                Moon.ApplyGravity(pair[0], pair[1]);

            foreach (var moon in moons)
                moon.ApplyVelocity();

            steps++;

            // if state of x on all moons is in its initial state, set xCycle to the amount of steps.
            if (moons.All(m => m.Velocity.X == 0 && m.Position.X == m.InitialPosition.X) && xCycle == 0)
                xCycle = steps;

            // if state of y on all moons is in its initial state, set yCycle to the amount of steps.
            if (moons.All(m => m.Velocity.Y == 0 && m.Position.Y == m.InitialPosition.Y) && yCycle == 0)
                yCycle = steps;

            // if state of z on all moons is in its initial state, set zCycle to the amount of steps.
            if (moons.All(m => m.Velocity.Z == 0 && m.Position.Z == m.InitialPosition.Z) && zCycle == 0)
                zCycle = steps;
        } while (xCycle == 0 || yCycle == 0 || zCycle == 0);

        // Find the least common multiplier between xCycle, yCycle and zCycle.
        var lcm = NumberTheory.LeastCommonMultiple(zCycle, xCycle, yCycle);

        return lcm.ToString();
    }

    private static Moon ParseLine(string line) => line
        .Extract<int, int, int>(@"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>")
        .Transform(m => new Moon(new Point3(m.First, m.Second, m.Third)));

    private class Moon(Point3 position)
    {
        public Point3 InitialPosition { get; } = position;

        public Point3 Position { get; set; } = position;

        public Point3 Velocity { get; set; } = Point3.Zero;

        public int PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);

        public int KineticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);

        public int TotalEnergy => PotentialEnergy * KineticEnergy;

        public void ApplyVelocity()
        {
            Position += Velocity;
        }

        public static void ApplyGravity(Moon a, Moon b)
        {
            var va = a.Velocity;
            var vb = b.Velocity;

            if (a.Position.X != b.Position.X)
            {
                if (a.Position.X < b.Position.X)
                {
                    va = va with { X = va.X + 1 };
                    vb = vb with { X = vb.X - 1 };
                }
                else
                {
                    va = va with { X = va.X - 1 };
                    vb = vb with { X = vb.X + 1 };
                }
            }

            if (a.Position.Y != b.Position.Y)
            {
                if (a.Position.Y < b.Position.Y)
                {
                    va = va with { Y = va.Y + 1 };
                    vb = vb with { Y = vb.Y - 1 };
                }
                else
                {
                    va = va with { Y = va.Y - 1 };
                    vb = vb with { Y = vb.Y + 1 };
                }
            }

            if (a.Position.Z != b.Position.Z)
            {
                if (a.Position.Z < b.Position.Z)
                {
                    va = va with { Z = va.Z + 1 };
                    vb = vb with { Z = vb.Z - 1 };
                }
                else
                {
                    va = va with { Z = va.Z - 1 };
                    vb = vb with { Z = vb.Z + 1 };
                }
            }

            a.Velocity = va;
            b.Velocity = vb;
        }
    }
}

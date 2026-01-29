using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(13)]
public class Challenge13
{
    [Part(1, "71,121")]
    public string Part1(string input)
    {
        var track = input.ToGrid();
        var carts = track.Where(kv => kv.Value is '>' or '<' or 'v' or '^').Select(ParseCart).ToList();

        while (true)
        {
            for (var i = 0; i < carts.Count; i++)
            {
                carts[i] = MoveOnTrack(track, carts[i]);
                if (carts.DistinctBy(x => x.Pose.Position).Count() != carts.Count)
                    return carts[i].Result;
            }
        }
    }

    [Part(2, "71,76")]
    public string Part2(string input)
    {
        var track = input.ToGrid();
        var carts = track.Where(kv => kv.Value is '>' or '<' or 'v' or '^').Select(ParseCart).ToList();

        while (true)
        {
            for (var i = 0; i < carts.Count; i++)
            {
                if (carts[i].Crashed)
                    continue;

                carts[i] = MoveOnTrack(track, carts[i]);

                int crash = carts
                    .Index()
                    .Where(c => c.Index != i && c.Item.Pose.Position == carts[i].Pose.Position && !c.Item.Crashed)
                    .Select(c => c.Index)
                    .FirstOrDefault(-1);

                if(crash >= 0)
                {
                    carts[i] = carts[i] with { Crashed = true };
                    carts[crash] = carts[crash] with { Crashed = true };
                }
            }

            if (carts.Where(c => !c.Crashed).Count() == 1)
            {
                var last = carts.Single(c => !c.Crashed);
                return last.Result;
            }
        }
    }

    private static Cart MoveOnTrack(Grid2<char> track, Cart cart)
    {
        // Move forward
        cart = cart with { Pose = cart.Pose.Step() };

        // Turn
        return (track[cart.Pose.Position], cart.Pose.Face) switch
        {
            ('+', _) => HandleIntersection(cart),
            ('/', var face) when face == Face.Up || face == Face.Down => cart with { Pose = cart.Pose.TurnRight() },
            ('/', var face) when face == Face.Left || face == Face.Right => cart with { Pose = cart.Pose.TurnLeft() },
            ('\\', var face) when face == Face.Up || face == Face.Down => cart with { Pose = cart.Pose.TurnLeft() },
            ('\\', var face) when face == Face.Left || face == Face.Right => cart with { Pose = cart.Pose.TurnRight() },
            _ => cart
        };
    }

    private static Cart HandleIntersection(Cart cart) => cart.Choice switch
    {
        0 => cart with { Pose = cart.Pose.TurnLeft(), Choice = (cart.Choice + 1).Mod(3) },
        2 => cart with { Pose = cart.Pose.TurnRight(), Choice = (cart.Choice + 1).Mod(3) },
        _ => cart with { Choice = (cart.Choice + 1).Mod(3) }
    };

    private static Cart ParseCart(KeyValuePair<Point2, char> kv)
    {
        return new Cart(new Pose2(kv.Key, kv.Value switch
        {
            'v' => Face.Down,
            '>' => Face.Right,
            '^' => Face.Up,
            '<' => Face.Left,
            _ => throw new NotImplementedException()
        }), 0);
    }

    public record Cart(Pose2 Pose, int Choice, bool Crashed = false)
    {
        public string Result => $"{Pose.Position.X},{Pose.Position.Y}";
    }
}

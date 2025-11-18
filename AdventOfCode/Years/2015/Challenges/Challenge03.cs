using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;

namespace AdventOfCode2015.Challenges;

[Challenge(3)]
public class Challenge03
{
    [Part(1, "2572")]
    public string Part1(string input)
    {
        Dictionary<Point2, int> map = [];
        var location = Point2.Zero;

        foreach (var c in input)
        {
            map.TryAdd(location, 0);
            map[location]++;
            location = c switch
            {
                '^' => location.Up,
                '>' => location.Right,
                'v' => location.Down,
                '<' => location.Left,
                _ => throw new NotImplementedException()
            };
        }

        return map.Count.ToString();
    }

    [Part(2, "2631")]
    public string Part2(string input)
    {
        Dictionary<Point2, int> map = [];
        Point2[] locations = [Point2.Zero, Point2.Zero];
        var index = 0;

        foreach (var c in input)
        {
            map.TryAdd(locations[index], 0);
            map[locations[index]]++;
            locations[index] = c switch
            {
                '^' => locations[index].Up,
                '>' => locations[index].Right,
                'v' => locations[index].Down,
                '<' => locations[index].Left,
                _ => throw new NotImplementedException()
            };

            index = (index + 1) % 2;
        }

        return map.Count.ToString();
    }
}

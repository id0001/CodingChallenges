using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(20)]
public class Challenge20
{
    [Part(1, "3476")]
    public string Part1(string input)
    {
        var map = MapRooms(input);
        var distances = Graph.Implicit<Room>(r => GetAjacent(map, r)).Bfs().Distances(map[Point2.Zero]);
        return distances.Values.Max().ToString();
    }

    [Part(2, "8514")]
    public string Part2(string input)
    {
        var map = MapRooms(input);
        var distances = Graph.Implicit<Room>(r => GetAjacent(map, r)).Bfs().Distances(map[Point2.Zero]);
        return distances.Values.Count(v => v >= 1000).ToString();
    }

    private static IEnumerable<(Room, Room)> GetAjacent(SpatialMap2<Room> map, Room source)
    {
        if (source.Doors[0])
            yield return (source, map[source.Location.Up]);

        if (source.Doors[1])
            yield return (source, map[source.Location.Right]);

        if (source.Doors[2])
            yield return (source, map[source.Location.Down]);

        if (source.Doors[3])
            yield return (source, map[source.Location.Left]);
    }

    private static SpatialMap2<Room> MapRooms(string input)
    {
        var map = new SpatialMap2<Room>();
        var stack = new Stack<Room>();
        var from = map.GetOrAdd(Point2.Zero, new Room(Point2.Zero));
        stack.Push(from);

        for (var i = 1; i < input.Length - 1; i++)
        {
            Room to;
            switch (input[i])
            {
                case 'N':
                    to = map.GetOrAdd(from.Location.Up, new Room(from.Location.Up));
                    MoveBetween(from, to);
                    from = to;
                    break;
                case 'E':
                    to = map.GetOrAdd(from.Location.Right, new Room(from.Location.Right));
                    MoveBetween(from, to);
                    from = to;
                    break;
                case 'S':
                    to = map.GetOrAdd(from.Location.Down, new Room(from.Location.Down));
                    MoveBetween(from, to);
                    from = to;
                    break;
                case 'W':
                    to = map.GetOrAdd(from.Location.Left, new Room(from.Location.Left));
                    MoveBetween(from, to);
                    from = to;
                    break;
                case '(':
                    stack.Push(from);
                    break;
                case ')':
                    from = stack.Pop();
                    break;
                case '|':
                    from = stack.Peek();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return map;
    }

    private static void MoveBetween(Room from, Room to)
    {
        if (from.Location.X == to.Location.X)
        {
            if (from.Location.Y > to.Location.Y)
            {
                from.Doors[0] = true;
                to.Doors[2] = true;
            }
            else
            {
                from.Doors[2] = true;
                to.Doors[0] = true;
            }
        }
        else
        {
            if (from.Location.X > to.Location.X)
            {
                from.Doors[3] = true;
                to.Doors[1] = true;
            }
            else
            {
                from.Doors[1] = true;
                to.Doors[3] = true;
            }
        }
    }

    private sealed class Room(Point2 location) : IEquatable<Room>
    {
        public Point2 Location { get; } = location;

        public bool[] Doors { get; } = new bool[4];

        public bool Equals(Room? other) => other is not null && Location.Equals(other.Location);
    }
}

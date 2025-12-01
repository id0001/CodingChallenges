using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;

namespace AdventOfCode2019.Challenges;

[Challenge(15)]
public class Challenge15
{
    [Part(1, "248")]
    public string Part1(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();

        var space = MapSpace(program);
        var target = space.First(x => x.Value == Node.Target);

        var path = Graph
            .Implicit<Point2>(n => GetAdjacent(space, n))
            .Bfs()
            .ShortestPath(Point2.Zero, target.Key);

        return (path.Length - 1).ToString();
    }

    [Part(2, "382")]
    public string Part2(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();

        var space = MapSpace(program);
        var target = space.First(x => x.Value == Node.Target);

        return Graph
            .Implicit<Point2>(n => GetAdjacent(space, n))
            .Bfs()
            .FloodFill(target.Key)
            .MaxBy(kv => kv.Distance)
            .Distance
            .ToString();
    }

    private static SpatialMap2<Node> MapSpace(long[] program)
    {
        var space = new SpatialMap2<Node>();
        space[Point2.Zero] = Node.Open;
        var cpu = new Cpu<long>(program);

        foreach (var neighbor in Point2.Zero.GetNeighbors())
            MapSpace(space, cpu, Point2.Zero, neighbor);

        return space;
    }

    private static void MapSpace(SpatialMap2<Node> space, Cpu<long> cpu, Point2 current, Point2 next)
    {
        long state = Move(cpu, next - current);

        switch (state)
        {
            case 0:
                space[next] = Node.Wall;
                return;
            case 1:
                space[next] = Node.Open;
                foreach (var neighbor in next.GetNeighbors())
                {
                    if (!space.ContainsPoint(neighbor))
                        MapSpace(space, cpu, next, neighbor);
                }
                Move(cpu, current - next);
                return;
            case 2:
                space[next] = Node.Target;
                Move(cpu, current - next);
                return;
            default:
                throw new NotImplementedException();
        }
    }

    private static IEnumerable<(Point2, Point2)> GetAdjacent(SpatialMap2<Node> space, Point2 current)
    {
        foreach (var neighbor in current.GetNeighbors())
        {
            if (space.ContainsPoint(neighbor) && space[neighbor] != Node.Wall)
                yield return (current, neighbor);
        }
    }

    private static long Move(Cpu<long> cpu, Point2 direction)
    {
        while (cpu.Next())
        {
            if (cpu.InputNeeded)
                cpu.Write(EncodeDirection(direction));

            if (cpu.HasOutput)
                return cpu.Read();
        }

        throw new InvalidOperationException();
    }

    private static int EncodeDirection(Point2 facing) => facing switch
    {
        (0, -1) => 1,
        (0, 1) => 2,
        (-1, 0) => 3,
        (1, 0) => 4,
        _ => throw new NotImplementedException()
    };

    private enum Node
    {
        Open,
        Wall,
        Target
    }
}

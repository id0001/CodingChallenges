using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Immutable;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(18)]
public class Challenge18
{
    private const int AllKeyMask = 0b0011_1111_1111_1111_1111_1111_1111;

    [Part(1, "3146")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();
        var center = grid.Single(x => x.Value == '@').Key;

        var maze = AnalyzeMaze(grid, center);

        var (path, cost) = Graph
            .Implicit<DroneState1, int>(c => GetAjacentPart1(grid, maze, c))
            .Dijkstra()
            .ShortestPath(new DroneState1(center, 0), c => IsFinished(c.ObtainedKeys));

        return cost.ToString();
    }

    [Part(2, "2194")]
    public string Part2(string input)
    {
        var grid = input.ToGrid();
        var center = grid.Single(x => x.Value == '@').Key;

        grid[center.Left] = '#';
        grid[center.Up] = '#';
        grid[center.Right] = '#';
        grid[center.Down] = '#';
        grid[center] = '#';

        var starts = new Point2[4];
        starts[0] = center + new Point2(-1, -1);
        starts[1] = center + new Point2(1, -1);
        starts[2] = center + new Point2(-1, 1);
        starts[3] = center + new Point2(1, 1);

        var maze = AnalyzeMaze(grid, starts);

        var start = new DroneState2(new ImmutableValueArray<Point2>(starts), 0);
        var (path, cost) = Graph
            .Implicit<DroneState2, int>(c => GetAjacentPart2(grid, maze, c))
            .Dijkstra()
            .ShortestPath(start, c => IsFinished(c.ObtainedKeys));

        return cost.ToString();
    }

    private static IEnumerable<(DroneState1, DroneState1, int)> GetAjacentPart1(Grid2<char> grid, ILookup<Point2, MazeEdge> edges, DroneState1 current)
    {
        foreach (var edge in edges[current.Position])
        {
            if (HasRequiredKeys(current.ObtainedKeys, edge.KeysRequired))
            {
                var keys = current.ObtainedKeys.SetBit(grid[edge.To] - 'a', true);
                yield return (current, new DroneState1(edge.To, keys), edge.Distance);
            }
        }
    }

    private static IEnumerable<(DroneState2, DroneState2, int)> GetAjacentPart2(Grid2<char> grid, ILookup<Point2, MazeEdge> edges, DroneState2 current)
    {
        for (var i = 0; i < 4; i++)
        {
            foreach (var edge in edges[current.Positions[i]])
            {
                if (HasRequiredKeys(current.ObtainedKeys, edge.KeysRequired))
                {
                    var keys = current.ObtainedKeys.SetBit(grid[edge.To] - 'a', true);
                    yield return (current, new DroneState2(current.Positions.SetItem(i, edge.To), keys), edge.Distance);
                }
            }
        }
    }

    private static ILookup<Point2, MazeEdge> AnalyzeMaze(Grid2<char> grid, params Point2[] starts)
    {
        var edges = new List<MazeEdge>();
        var keys = grid.Where(kv => char.IsAsciiLetterLower(kv.Value)).Select(x => x.Key).ToList();

        foreach (var vertex in starts.Concat(keys))
            foreach (var edge in FindEdges(grid, vertex))
                edges.Add(edge);

        return edges.ToLookup(kv => kv.From);
    }

    /// <summary>
    /// Flood fill to find all reachable vertices
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    private static IEnumerable<MazeEdge> FindEdges(Grid2<char> grid, Point2 from)
    {
        Queue<Point2> queue = new([from]);
        Dictionary<Point2, (int, int)> visited = new() { [from] = (0, 0) };

        while (queue.Count > 0)
        {
            var currentVertex = queue.Dequeue();
            var (distance, keys) = visited[currentVertex];

            if (from != currentVertex && char.IsAsciiLetterLower(grid[currentVertex]))
            {
                yield return new MazeEdge(from, currentVertex, keys, distance);
                continue;
            }

            foreach (var nextEdge in GetAdjacentOnGrid(grid, currentVertex))
            {
                if (visited.ContainsKey(nextEdge.Target))
                    continue;

                var newKeys = keys;
                if (char.IsAsciiLetterUpper(grid[nextEdge.Target]))
                    newKeys = newKeys.SetBit(grid[nextEdge.Target] - 'A', true);

                visited.Add(nextEdge.Target, (distance + 1, newKeys));
                queue.Enqueue(nextEdge.Target);
            }
        }
    }

    private static IEnumerable<(Point2 Source, Point2 Target)> GetAdjacentOnGrid(Grid2<char> grid, Point2 current)
    {
        foreach (var neighbor in current.GetNeighbors())
        {
            if (grid[neighbor] != '#')
                yield return (current, neighbor);
        }
    }

    private static bool HasRequiredKeys(int obtainedKeys, int requiredKeys) => (obtainedKeys & requiredKeys) == requiredKeys;

    private static bool IsFinished(int obtainedKeys) => (obtainedKeys & AllKeyMask) == AllKeyMask;

    private record MazeEdge(Point2 From, Point2 To, int KeysRequired, int Distance);
    private record DroneState1(Point2 Position, int ObtainedKeys);
    private record DroneState2(ImmutableValueArray<Point2> Positions, int ObtainedKeys);
}

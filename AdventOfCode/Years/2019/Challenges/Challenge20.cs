using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;
using System.Numerics;

namespace AdventOfCode2019.Challenges;

[Challenge(20)]
public class Challenge20
{
    [Part(1, "658")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();

        var maze = AnalyzeMaze(grid);
        var start = maze.First(x => x.Key.Name == "AA").Key;

        var (path, cost) = Graph
            .Implicit<Portal, int>(c => GetAdjacent(maze, c))
            .Dijkstra()
            .ShortestPath(start, c => c.Name == "ZZ");

        return cost.ToString();
    }

    [Part(2, "7612")]
    public string Part2(string input)
    {
        var grid = input.ToGrid();

        var maze = AnalyzeMaze(grid);
        var start = maze.First(x => x.Key.Name == "AA").Key;

        var (path, cost) = Graph
            .Implicit<PortalWithLevel, int>(c => GetAdjacent2(maze, c))
            .Dijkstra()
            .ShortestPath(new PortalWithLevel(start, 0), c => c.Portal.Name == "ZZ" && c.Level == 0);

        return cost.ToString();
    }

    private static IEnumerable<(Portal, Portal, int)> GetAdjacent(ILookup<Portal, (Portal, Portal, int)> edges, Portal current) => edges[current];

    private static IEnumerable<(PortalWithLevel, PortalWithLevel, int)> GetAdjacent2(ILookup<Portal, (Portal Source, Portal Target, int Weight)> edges, PortalWithLevel current)
    {
        foreach (var edge in edges[current.Portal])
        {
            if (edge.Target.Name == edge.Source.Name)
            {
                // Going through a portal
                // Outer -> Inner == Up
                // Inner -> Outer == Down

                if (current.Level == 0 && edge.Source.Type == PortalType.Outer)
                    continue; // Can't go beyond outer most layer.

                int direction = edge.Target.Type == PortalType.Outer ? 1 : -1;
                yield return (current, new PortalWithLevel(edge.Target, current.Level + direction), edge.Weight);
            }
            else
            {
                // Moving on level
                yield return (current, new PortalWithLevel(edge.Target, current.Level), edge.Weight);
            }
        }
    }

    private static ILookup<Portal, (Portal, Portal, int)> AnalyzeMaze(Grid2<char> grid)
    {
        var portals = new Dictionary<Point2, Portal>();
        var edges = new List<(Portal Source, Portal Target, int Weight)>();

        foreach (var (p, c) in grid)
        {
            if (c == '.')
            {
                foreach (var neighbor in p.GetNeighbors().Where(n => char.IsAsciiLetterUpper(grid[n])))
                {
                    var dir = neighbor - p;
                    var name = GetName(grid, neighbor, dir);
                    var isOuter = p.X == 2 || p.X == grid.Columns - 3 || p.Y == 2 || p.Y == grid.Rows - 3;

                    portals.Add(p, new Portal(p, name, isOuter ? PortalType.Outer : PortalType.Inner));
                }
            }
        }

        // Portals link to their namesake
        foreach (var group in portals.Values.GroupBy(x => x.Name))
        {
            var values = group.ToArray();
            if (values.Length < 2)
                continue;

            edges.Add((values[0], values[1], 1));
            edges.Add((values[1], values[0], 1));
        }

        foreach (var portal in portals.Values)
        {
            foreach (var edge in FindEdges(grid, portals, portal))
                edges.Add(edge);
        }

        return edges.ToLookup(kv => kv.Source);
    }

    private static IEnumerable<(Point2 Source, Point2 Target)> GetAdjacentOnGrid(Grid2<char> grid, Point2 current)
    {
        foreach (var neighbor in current.GetNeighbors())
        {
            if (grid[neighbor] != '#' && !char.IsAsciiLetterUpper(grid[neighbor]))
                yield return (current, neighbor);
        }
    }

    private static IEnumerable<(Portal, Portal, int)> FindEdges(Grid2<char> grid, Dictionary<Point2, Portal> portals, Portal from)
    {
        Queue<Point2> queue = new([from.Location]);
        Dictionary<Point2, int> visited = new() { [from.Location] = 0 };

        while (queue.Count > 0)
        {
            var currentVertex = queue.Dequeue();
            var distance = visited[currentVertex];

            if (from.Location != currentVertex && portals.ContainsKey(currentVertex))
            {
                yield return (from, portals[currentVertex], distance);
                continue;
            }

            foreach (var nextEdge in GetAdjacentOnGrid(grid, currentVertex))
            {
                if (visited.ContainsKey(nextEdge.Target))
                    continue;

                visited.Add(nextEdge.Target, distance + 1);
                queue.Enqueue(nextEdge.Target);
            }
        }
    }

    private static string GetName(Grid2<char> grid, Point2 location, Point2 direction) => direction.X > 0 ^ direction.Y > 0
        ? string.Join(string.Empty, grid[location], grid[location + direction])
        : string.Join(string.Empty, grid[location + direction], grid[location]);

    private record Portal(Point2 Location, string Name, PortalType Type);

    private record PortalWithLevel(Portal Portal, int Level);

    private enum PortalType
    {
        Inner,
        Outer
    }
}

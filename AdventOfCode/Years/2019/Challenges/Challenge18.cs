using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;
using System.Net.WebSockets;

namespace AdventOfCode2019.Challenges;

[Challenge(18)]
public class Challenge18
{
    private const int AllKeyMask = 0b0011_1111_1111_1111_1111_1111_1111;

    [Part(1, "3146")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();
        var edges = AnalyzeMaze(grid);

        var (path, cost) = Graph.ImplicitWeighted<Node, int>(c => GetAdjacent(edges, c)).Dijkstra().ShortestPath(new Node('@', 0), n => (n.KeysObtained & AllKeyMask) == AllKeyMask);
        return (cost).ToString();
    }

    // [Part(2, "2194")]
    //public string Part2(string input)
    //{
    //    throw new NoResultException();
    //}

    private static IEnumerable<WeightedEdge<Node, int>> GetAdjacent(ILookup<char, Edge> edges, Node current)
    {
        foreach (var edge in edges[current.Id])
        {
            if ((edge.KeysRequired & current.KeysObtained) == edge.KeysRequired)
            {
                int keys = current.KeysObtained.SetBit(edge.To - 'a', true);
                yield return new WeightedEdge<Node, int>(current, new Node(edge.To, keys), edge.Distance);
            }
        }
    }

    private static ILookup<char, Edge> AnalyzeMaze(Grid2<char> grid)
    {
        var keys = new Dictionary<Point2, char>();
        var doors = new Dictionary<Point2, char>();
        Point2 start = Point2.Zero;
        foreach (var (p, c) in grid)
        {
            switch (c)
            {
                case var _ when char.IsUpper(c):
                    doors.Add(p, c);
                    break;
                case var _ when char.IsLower(c):
                    keys.Add(p, c);
                    break;
                case '@':
                    start = p;
                    break;
                default:
                    break;
            }
        }

        var edges = new List<Edge>();
        var bfs = Graph.Implicit<Point2>(c => GetAdjacentOnGrid(grid, c)).Bfs();

        var lookup = Flood(grid, start);
        foreach (var key in keys.Keys)
        {
            var (distance, reqKeys) = lookup[key];
            edges.Add(new Edge('@', keys[key], reqKeys, distance));
        }

        var lookupAll = new Dictionary<Point2, Dictionary<Point2, (int, int)>>();
        foreach(var pair in keys.Keys.Combinations(2))
        {
            if (!lookupAll.TryGetValue(pair[0], out lookup))
            {
                lookup = Flood(grid, pair[0]);
                lookupAll.Add(pair[0], lookup);
            }

            var (distance, reqKeys) = lookup[pair[1]];
            edges.Add(new Edge(keys[pair[0]], keys[pair[1]], reqKeys, distance));
            edges.Add(new Edge(keys[pair[1]], keys[pair[0]], reqKeys, distance));
        }

        return edges.ToLookup(kv => kv.From);
    }


    private static IEnumerable<Edge<Point2>> GetAdjacentOnGrid(Grid2<char> grid, Point2 current)
    {
        foreach (var neighbor in current.GetNeighbors())
        {
            if (grid[neighbor] != '#')
                yield return new Edge<Point2>(current, neighbor);
        }
    }

    private static Dictionary<Point2, (int Distance, int Keys)> Flood(Grid2<char> grid, Point2 start)
    {
        Queue<Point2> queue = new([start]);
        Dictionary<Point2, (int, int)> visited = new() { [start] = (0, 0) };

        var result = new List<(Point2 Vertex, int Distance, int Keys)>();

        while (queue.Count > 0)
        {
            var currentVertex = queue.Dequeue();
            var (distance, keys) = visited[currentVertex];

            if (char.IsLower(grid[currentVertex]))
                result.Add((currentVertex, distance, keys));

            foreach (var nextEdge in GetAdjacentOnGrid(grid, currentVertex))
            {
                if (visited.ContainsKey(nextEdge.Target))
                    continue;

                var newKeys = keys;
                if (char.IsUpper(grid[nextEdge.Target]))
                    newKeys = keys.SetBit(grid[nextEdge.Target] - 'A', true);

                visited.Add(nextEdge.Target, (distance + 1, newKeys));
                queue.Enqueue(nextEdge.Target);
            }
        }

        return result.ToDictionary(kv => kv.Vertex, kv => (kv.Distance, kv.Keys));
    }

    private record Node(char Id, int KeysObtained);

    private record Edge(char From, char To, int KeysRequired, int Distance);
}

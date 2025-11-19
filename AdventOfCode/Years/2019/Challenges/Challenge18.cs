using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;

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

    private static IEnumerable<WeightedEdge<Node,int>> GetAdjacent(ILookup<char, Edge> edges, Node current)
    {
        foreach(var edge in edges[current.Id])
        {
            if((edge.KeysRequired & current.KeysObtained) == edge.KeysRequired)
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

        foreach (var key in keys.Keys)
        {
            var path = bfs.ShortestPath(start, key);
            int keysRequired = 0;
            foreach (var p in path)
            {
                if (doors.ContainsKey(p))
                    keysRequired = keysRequired.SetBit(doors[p] - 'A', true);
            }

            edges.Add(new Edge('@', keys[key], keysRequired, path.Length - 1));
        }

        foreach (var from in keys.Keys)
        {
            foreach (var to in keys.Keys)
            {
                var path = bfs.ShortestPath(from, to);
                int keysRequired = 0;
                foreach (var p in path)
                {
                    if (doors.ContainsKey(p))
                        keysRequired = keysRequired.SetBit(doors[p] - 'A', true);
                }

                edges.Add(new Edge(keys[from], keys[to], keysRequired, path.Length - 1));
            }
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

    private record Node(char Id, int KeysObtained);

    private record Edge(char From, char To, int KeysRequired, int Distance);
}

using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Challenges;

[Challenge(17)]
public class Challenge17
{
    private static readonly MD5 Hasher = MD5.Create();
    private static readonly char[] PassDirections = ['U', 'D', 'L', 'R'];
    private static readonly Point2[] Directions = [Face.Up, Face.Down, Face.Left, Face.Right];
    private static readonly Rectangle Bounds = new(0, 0, 4, 4);

    [Part(1, "RDDRLDRURD")]
    public string Part1(string input)
    {
        var start = new Node(Point2.Zero, input);

        var path = Graph.Implicit<Node>(GetAdjacent).Bfs().ShortestPath(start, n => n.Position == new Point2(3, 3));
        return path.Last().Passcode[input.Length..];
    }

    [Part(2, "448")]
    public string Part2(string input)
    {
        var start = new Node(Point2.Zero, input);

        var path = Graph.Implicit<Node>(GetAdjacent).Dfs().LongestPath(start, n => n.Position == new Point2(3, 3));
        return (path.Length - 1).ToString();
    }

    private static IEnumerable<Edge<Node>> GetAdjacent(Node current)
    {
        var hash = GetHash(current.Passcode);

        for (var i = 0; i < 4; i++)
        {
            if (hash[i] == 'a' || char.IsNumber(hash[i]) || !Bounds.Contains(current.Position + Directions[i]))
                continue;

            yield return new Edge<Node>(current, new Node(current.Position + Directions[i], current.Passcode + PassDirections[i]));
        }
    }

    private static string GetHash(string passcode) =>
        Convert.ToHexString(Hasher.ComputeHash(Encoding.Default.GetBytes(passcode))).ToLowerInvariant();

    private record Node(Point2 Position, string Passcode);
}

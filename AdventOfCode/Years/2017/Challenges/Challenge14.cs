using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;
using System.Collections;
using System.Numerics;

namespace AdventOfCode2017.Challenges;

[Challenge(14)]
public class Challenge14
{
    [Part(1, "8204")]
    public string Part1(string input) => Enumerable
        .Range(0, 128)
        .Select(i => KnotHash.Generate($"{input}-{i}").Sum(b => BitOperations.PopCount(b)))
        .Sum()
        .ToString();

    [Part(2, "1089")]
    public string Part2(string input)
    {
        var bits = new BitArray(Enumerable
            .Range(0, 128)
            .SelectMany(i => KnotHash.Generate($"{input}-{i}"))
            .Reverse()
            .ToArray());

        var regionCount = 0;
        var bounds = new Rectangle(0, 0, 128, 128);
        var bfs = Graph.Implicit<Point2>(n => GetAdjacent(bits, bounds, n)).Bfs();

        for(var unvisited = 0; unvisited < bits.Length;  unvisited++)
        {
            if (!bits[unvisited])
                continue;

            regionCount++;
            foreach (var (i, _) in bfs.FloodFill(unvisited.ToPoint2(128)))
                bits[i.ToIndex(128)] = false;
        }

        return regionCount.ToString();
    }

    private static IEnumerable<(Point2, Point2)> GetAdjacent(BitArray bits, Rectangle bounds, Point2 current)
    {
        foreach(var neighbor in current.GetNeighbors().Where(bounds.Contains))
        {
            var index = neighbor.ToIndex(128);
            if (bits[index])
                yield return (current, neighbor);
        }
    }
}

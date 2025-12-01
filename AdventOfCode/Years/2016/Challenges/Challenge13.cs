using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;

namespace AdventOfCode2016.Challenges;

[Challenge(13)]
public class Challenge13
{
    [Part(1, "90")]
    public string Part1(string input) => (Graph.Implicit<Point2>(c => GetAjacent(c, input.As<int>())).Bfs().ShortestPath(Point2.One, new Point2(31, 39)).Length - 1).ToString();

    [Part(2, "135")]
    public string Part2(string input)
        => Graph.Implicit<Point2>(c => GetAjacent(c, input.As<int>())).Bfs().FloodFill(Point2.One, 50).Count().ToString();

    private static IEnumerable<(Point2, Point2)> GetAjacent(Point2 current, int input)
        => current.GetNeighbors().Where(p => p.X >= 0 && p.Y >= 0 && !IsWall(p, input)).Select(p => (current, p));

    private static bool IsWall(Point2 p, int input) => Convert.ToString(p.X * p.X + 3 * p.X + 2 * p.X * p.Y + p.Y + p.Y * p.Y + input, 2).Count(c => c == '1') % 2 != 0;
}

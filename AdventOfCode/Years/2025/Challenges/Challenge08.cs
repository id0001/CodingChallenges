using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(8)]
public class Challenge08
{
    [Part(1, "84968")]
    public string Part1(string input)
    {
        var vertices = input.Lines(line => line.SplitBy<int>(",").Transform(v => new Point3(v.First(), v.Second(), v.Third()))).ToList();

        var dsu = new UnionFind<Point3>(vertices);
        foreach (var (a, b, _) in EnumerateDistances(vertices).OrderBy(e => e.Distance).Take(1000))
            dsu.Union(a, b);

        return dsu.GetAllSets().OrderByDescending(x => x.Count).Take(3).Select(x => x.Count).Product().ToString();
    }

    [Part(2, "8663467782")]
    public string Part2(string input)
    {
        var vertices = input.Lines(line => line.SplitBy<int>(",").Transform(v => new Point3(v.First(), v.Second(), v.Third()))).ToList();

        int vertexCount = vertices.Count;
        var dsu = new UnionFind<Point3>(vertices);
        foreach (var (a, b, _) in EnumerateDistances(vertices).OrderBy(e => e.Distance))
        {
            if (!dsu.AreConnected(a, b))
            {
                dsu.Union(a, b);
                vertexCount--;
            }

            if (vertexCount == 1)
                return (a.X * (long)b.X).ToString();
        }

        throw new InvalidOperationException();
    }


    private static IEnumerable<(Point3 Source, Point3 Target, long Distance)> EnumerateDistances(List<Point3> vertices)
    {
        foreach (var pair in vertices.Combinations(2))
            yield return (pair[0], pair[1], Point3.DistanceSquared(pair[0], pair[1]));
    }
}

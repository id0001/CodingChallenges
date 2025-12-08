using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(8)]
public class Challenge08
{
    [Part(1, "84968")]
    public string Part1(string input)
    {
        var vertices = input.Lines(line => line.SplitBy<int>(",").Transform(v => new Point3(v.First(), v.Second(), v.Third()))).ToList();

        var circuits = new List<List<Point3>>();
        foreach (var v in vertices)
            circuits.Add([v]);

        foreach (var (a,b, _) in EnumerateDistances(vertices).OrderBy(e => e.Distance).Take(1000))
        {
            List<Point3> c1 = null!;
            List<Point3> c2 = null!;
            
            foreach(var circuit in circuits)
            {
                if (circuit.Contains(a))
                    c1 = circuit;

                if (circuit.Contains(b))
                    c2 = circuit;
            }

            if(c1 != c2)
            {
                c1.AddRange(c2);
                circuits.Remove(c2);
            }
        }

        return circuits.OrderByDescending(x => x.Count).Take(3).Select(x => x.Count).Product().ToString();
    }

    [Part(2, "8663467782")]
    public string Part2(string input)
    {
        var vertices = input.Lines(line => line.SplitBy<int>(",").Transform(v => new Point3(v.First(), v.Second(), v.Third()))).ToList();

        var circuits = new List<List<Point3>>();
        foreach (var v in vertices)
            circuits.Add([v]);

        foreach (var (a, b, _) in EnumerateDistances(vertices).OrderBy(e => e.Distance))
        {
            List<Point3> c1 = null!;
            List<Point3> c2 = null!;

            foreach (var circuit in circuits)
            {
                if (circuit.Contains(a))
                    c1 = circuit;

                if (circuit.Contains(b))
                    c2 = circuit;
            }

            if (c1 != c2)
            {
                if (circuits.Count == 2)
                    return (a.X * (long)b.X).ToString();

                c1.AddRange(c2);
                circuits.Remove(c2);
            }
        }

        throw new InvalidOperationException();
    }


    private static IEnumerable<(Point3 Source, Point3 Target, long Distance)> EnumerateDistances(List<Point3> vertices)
    {
        foreach (var pair in vertices.Combinations(2))
            yield return (pair[0], pair[1], Point3.DistanceSquared(pair[0], pair[1]));
    }
}

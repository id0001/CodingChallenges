using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;
using System.Runtime.CompilerServices;

namespace AdventOfCode2019.Challenges;

[Challenge(24)]
public class Challenge24
{
    [Part(1, "32513278")]
    public string Part1(string input)
    {
        var readgrid = input.ToGrid();
        var writegrid = new Grid2<char>(readgrid.RowCount, readgrid.ColumnCount);

        var history = new HashSet<string>();
        var dataString = readgrid.ToString();
        while (!history.Contains(dataString))
        {
            history.Add(dataString);

            for (var y = 0; y < 5; y++)
            {
                for (var x = 0; x < 5; x++)
                {
                    var p = new Point2(x, y);
                    var bc = 0;

                    foreach (var neighbor in p.GetNeighbors())
                    {
                        if (!readgrid.Bounds.Contains(neighbor))
                            continue;

                        if (readgrid[neighbor.Y, neighbor.X] == '#')
                            bc++;
                    }

                    writegrid[p] = readgrid[p] switch
                    {
                        '#' when bc != 1 => '.',
                        '.' when bc is >= 1 and <= 2 => '#',
                        _ => readgrid[p]
                    };
                }
            }

            (readgrid, writegrid) = (writegrid, readgrid);
            dataString = readgrid.ToString();
        }

        return GetBiodiversity(readgrid).ToString();
    }

    [Part(2, "1912")]
    public string Part2(string input)
    {
        const int totalGen = 200;
        var upperBound = 1;
        var lowerBound = -1;

        var read = new Dictionary<int, Grid2<char>>
        {
            {0, input.ToGrid() },
            {-1, Grid2.Fill(5,5,'.') },
            {1, Grid2.Fill(5,5,'.') }
        };

        var emptyGridString = Grid2.Fill(5, 5, '.').ToString();

        for (var gen = 0; gen < totalGen; gen++)
        {
            var write = new Dictionary<int, Grid2<char>>();

            for (var level = lowerBound; level <= upperBound; level++)
            {
                if (!write.ContainsKey(level))
                    write.Add(level, Grid2.Fill(5, 5, '.'));

                for (var y = 0; y < 5; y++)
                {
                    for (var x = 0; x < 5; x++)
                    {
                        if (x == 2 && y == 2)
                            continue;

                        var p = new Point3(x, y, level);
                        var bc = 0;

                        foreach (var neighbor in GetNeighbors(p))
                        {
                            if (!read.ContainsKey(neighbor.Z))
                                read.Add(neighbor.Z, Grid2.Fill(5, 5, '.'));

                            if (read[neighbor.Z][neighbor.Y, neighbor.X] == '#')
                                bc++;
                        }

                        write[level][p.ToPoint2()] = read[level][p.ToPoint2()] switch
                        {
                            '#' when bc != 1 => '.',
                            '.' when bc is >= 1 and <= 2 => '#',
                            _ => read[level][p.ToPoint2()]
                        };
                    }
                }
            }

            lowerBound--;
            upperBound++;

            // reduce levels
            if (!read.ContainsKey(lowerBound + 1) || read[lowerBound + 1].ToString() == emptyGridString)
            {
                read.Remove(lowerBound);
                lowerBound++;
            }

            if (!read.ContainsKey(upperBound - 1) || read[upperBound - 1].ToString() == emptyGridString)
            {
                read.Remove(upperBound);
                upperBound--;
            }

            read = write;
        }

        return read.Sum(kv => kv.Value.Count(kv => kv.Value == '#')).ToString();
    }

    private static IEnumerable<Point3> GetNeighbors(Point3 current)
    {
        foreach (var neighbor in current.ToPoint2().GetNeighbors())
            foreach (var n in GetNeighbors(current, neighbor))
                yield return n;
    }

    private static IEnumerable<Point3> GetNeighbors(Point3 current, Point3 neighbor) => (neighbor.X, neighbor.Y) switch
    {
        (-1, _) => [new Point3(1, 2, current.Z - 1)],
        (5, _) => [new Point3(3, 2, current.Z - 1)],
        (_, -1) => [new Point3(2, 1, current.Z - 1)],
        (_, 5) => [new Point3(2, 3, current.Z - 1)],
        (2, 2) => (current.X, current.Y) switch
        {
            (1, _) => Enumerable.Range(0, 5).Select(y => new Point3(0, y, current.Z + 1)),
            (3, _) => Enumerable.Range(0, 5).Select(y => new Point3(4, y, current.Z + 1)),
            (_, 1) => Enumerable.Range(0, 5).Select(x => new Point3(x, 0, current.Z + 1)),
            (_, 3) => Enumerable.Range(0, 5).Select(x => new Point3(x, 4, current.Z + 1)),
            _ => throw new NotImplementedException()
        },
        _ => [new Point3(neighbor.X, neighbor.Y, current.Z)]
    };

    private static int GetBiodiversity(Grid2<char> data)
    {
        var diversity = 0;
        foreach (var (i, (k, v)) in data.Index())
        {
            if (v == '#')
                diversity += (int)Math.Pow(2, i);
        }

        return diversity;
    }
}

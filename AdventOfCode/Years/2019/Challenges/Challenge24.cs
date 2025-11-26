using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(24)]
public class Challenge24
{
    [Part(1, "32513278")]
    public string Part1(string input)
    {
        var readgrid = input.ToGrid();
        var writegrid = new Grid2<char>(readgrid.Rows, readgrid.Columns);

        var history = new HashSet<string>();
        var dataString = readgrid.ToString((_, c) => c);
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
            dataString = readgrid.ToString((_, c) => c);
        }

        return GetBiodiversity(readgrid).ToString();
    }

    //// [Part(2, "1912")]
    //public string Part2(string input)
    //{
    //    throw new NoResultException();
    //}

    private static int GetBiodiversity(Grid2<char> data)
    {
        var diversity = 0;
        foreach(var (i, (k, v)) in data.Index())
        {
            if (v == '#')
                diversity += (int)Math.Pow(2, i);
        }

        return diversity;
    }
}

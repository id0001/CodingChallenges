using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace EverybodyCodes2024.Challenges;

[Challenge(3)]
public class Quest03
{
    [Part(1, "116")]
    public string Part1(string input) => CalculateDugUnits(input.ToGrid(), false).ToString();

    [Part(2, "2678")]
    public string Part2(string input) => CalculateDugUnits(input.ToGrid(), false).ToString();

    [Part(3, "10698")]
    public string Part3(string input) => CalculateDugUnits(input.ToGrid(), true).ToString();

    private static int CalculateDugUnits(Grid2<char> grid, bool includeDiagonal)
    {
        var readmap = new SpatialMap2<int>();
        foreach (var (p, c) in grid)
        {
            if (c == '#')
                readmap[p] = 0;
        }

        var writemap = new SpatialMap2<int>();

        int sum = 0;
        for (var round = 0; ; round++)
        {
            int dug = 0;
            foreach (var (p, v) in readmap)
            {
                writemap[p] = v;

                if (v != round)
                    continue;

                if (p.GetNeighbors(includeDiagonal).All(n => readmap.GetValueOrDefault(n, 0) == round))
                {
                    writemap[p] = v + 1;
                    dug++;
                }
            }

            sum += dug;
            if (dug == 0)
                break;

            (readmap, writemap) = (writemap, readmap);
        }

        return sum;
    }
}

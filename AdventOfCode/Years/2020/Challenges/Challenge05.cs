using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2020.Challenges;

[Challenge(5)]
public class Challenge05
{
    [Part(1, "835")]
    public string Part1(string input)
    {
        var lines = input.Lines().ToList();

        return lines
            .Select(query => Search(8, 128, query))
            .Select(p => p.Y * 8 + p.X)
            .Prepend(0)
            .Max()
            .ToString();
    }

    [Part(2, "649")]
    public string Part2(string input)
    {
        var lines = input.Lines().ToList();

        var seatIds = new HashSet<int>();
        foreach (var query in lines)
        {
            var p = Search(8, 128, query);
            var id = p.Y * 8 + p.X;
            seatIds.Add(id);
        }

        var mySeatId = Enumerable
            .Range(0, 128 * 8)
            .First(n => !seatIds.Contains(n) && seatIds.Contains(n - 1) && seatIds.Contains(n + 1));

        return mySeatId.ToString();
    }

    private static Point2 Search(int width, int height, string query)
    {
        int xmin = 0, ymin = 0, xmax = width, ymax = height;
        foreach (var c in query)
        {
            switch (c)
            {
                case 'F':
                    ymax = (ymax + ymin) / 2;
                    break;
                case 'B':
                    ymin = (ymax + ymin) / 2;
                    break;
                case 'L':
                    xmax = (xmax + xmin) / 2;
                    break;
                case 'R':
                    xmin = (xmax + xmin) / 2;
                    break;
            }
        }

        return new Point2(xmin, ymin);
    }
}

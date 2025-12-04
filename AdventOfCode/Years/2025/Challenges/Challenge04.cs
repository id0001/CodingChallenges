using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(4)]
public class Challenge04
{
    [Part(1, "1547")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();

        return grid.Count(kv => IsAccessable(grid, kv.Key)).ToString();
    }

    [Part(2, "8948")]
    public string Part2(string input)
    {
        var grid = input.ToGrid();
        int sum = 0;
        bool removedAny = false;
        do
        {
            removedAny = false;
            foreach (var (p,_) in grid.Where(kv => kv.Value == '@'))
            {
                if (IsAccessable(grid, p))
                {
                    grid[p] = '.';
                    sum++;
                    removedAny = true;
                }
            }
        }
        while (removedAny);

        return sum.ToString();
    }

    private bool IsAccessable(Grid2<char> grid, Point2 loc)
    {
        if (grid[loc] != '@')
            return false;

        return loc.GetNeighbors(true).Where(grid.Bounds.Contains).Count(p => grid[p] == '@') < 4;
    }
}

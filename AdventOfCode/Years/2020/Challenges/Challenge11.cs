using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2020.Challenges;

[Challenge(11)]
public class Challenge11
{
    [Part(1, "2481")]
    public string Part1(string input)
    {
        var state = input.ToGrid();

        var changed = false;
        do
        {
            changed = false;
            var next = new Grid2<char>(state.RowCount, state.ColumnCount);
            foreach (var (p, c) in state)
            {
                changed |= c switch
                {
                    '.' => Ignore(next, p),
                    'L' => Occupy1(state, next, p),
                    '#' => Empty1(state, next, p),
                    _ => throw new NotImplementedException()
                };
            }

            state = next;
        } while (changed);

        return state.Count(x => x.Value == '#').ToString();
    }

    [Part(2, "2227")]
    public string Part2(string input)
    {
        var state = input.ToGrid();

        var changed = false;
        do
        {
            changed = false;
            var next = new Grid2<char>(state.RowCount, state.ColumnCount);
            foreach (var (p, c) in state)
            {
                changed |= c switch
                {
                    '.' => Ignore(next, p),
                    'L' => Occupy2(state, next, p),
                    '#' => Empty2(state, next, p),
                    _ => throw new NotImplementedException()
                };
            }

            state = next;
        } while (changed);

        return state.Count(x => x.Value == '#').ToString();
    }

    private static bool Ignore(Grid2<char> next, Point2 p)
    {
        next[p] = '.';
        return false;
    }

    private static bool Occupy1(Grid2<char> state, Grid2<char> next, Point2 p)
    {
        if (p.GetNeighbors(true).Where(state.Bounds.Contains).Any(n => state[n] == '#'))
        {
            next[p] = 'L';
            return false;
        }

        next[p] = '#';
        return true;
    }

    private static bool Occupy2(Grid2<char> state, Grid2<char> next, Point2 p)
    {
        if (NeighborsInLineOfSight(p, state).Any(n => state[n] == '#'))
        {
            next[p] = 'L';
            return false;
        }

        next[p] = '#';
        return true;
    }

    private static bool Empty1(Grid2<char> state, Grid2<char> next, Point2 p)
    {
        if (p.GetNeighbors(true).Where(state.Bounds.Contains).Count(n => state[n] == '#') >= 4)
        {
            next[p] = 'L';
            return true;
        }

        next[p] = '#';
        return false;
    }

    private static bool Empty2(Grid2<char> state, Grid2<char> next, Point2 p)
    {
        if (NeighborsInLineOfSight(p, state).Count(n => state[n] == '#') >= 5)
        {
            next[p] = 'L';
            return true;
        }

        next[p] = '#';
        return false;
    }

    private static IEnumerable<Point2> NeighborsInLineOfSight(Point2 p, Grid2<char> state)
    {
        foreach (var direction in Point2.Zero.GetNeighbors(true))
        {
            for (var i = 1; ; i++)
            {
                var neighbor = p + (direction * i);
                if (!state.Bounds.Contains(neighbor))
                    break;

                if (state[neighbor] != '.')
                {
                    yield return neighbor;
                    break;
                }
            }
        }
    }
}

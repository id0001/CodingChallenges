using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(18)]
public class Challenge18
{
    private static readonly Rectangle Bounds = new(0, 0, 50, 50);

    [Part(1, "644640")]
    public string Part1(string input)
    {
        var state = input.ToGrid();
        for (var i = 0; i < 10; i++)
        {
            var next = new Grid2<char>(state.RowCount, state.ColumnCount);

            foreach (var (p, _) in state)
                next[p] = ExecuteAutomata(state, p);

            state = next;
        }

        var (tree, lumberyard) = Count(state);
        return (tree * lumberyard).ToString();
    }

    [Part(2, "191080")]
    public string Part2(string input)
    {
        var state = input.ToGrid();
        var previousStates = new Dictionary<string, int>
        {
            { state.ToString(), 0 }
        };

        var total = 1_000_000_000;
        for (var i = 0; i < total; i++)
        {
            var next = new Grid2<char>(state.RowCount, state.ColumnCount);

            foreach (var (p, _) in state)
                next[p] = ExecuteAutomata(state, p);

            state = next;

            var key = state.ToString();
            if (previousStates.ContainsKey(key))
            {
                var cycleLength = i - previousStates[key];
                i += (total - i) / cycleLength * cycleLength;
                continue;
            }

            previousStates.Add(key, i);
        }

        var (tree, lumberyard) = Count(state);
        return (tree * lumberyard).ToString();
    }

    private char ExecuteAutomata(Grid2<char> state, Point2 p) => state[p] switch
    {
        '.' => OpenAcre(state, p),
        '|' => Tree(state, p),
        '#' => Lumberyard(state, p),
        _ => throw new NotImplementedException()
    };

    private static char OpenAcre(Grid2<char> state, Point2 p) => p.GetNeighbors(true).Where(IsInBounds).Count(n => state[n] == '|') >= 3 ? '|' : '.';

    private static char Tree(Grid2<char> state, Point2 p) => p.GetNeighbors(true).Where(IsInBounds).Count(n => state[n] == '#') >= 3 ? '#' : '|';

    private static char Lumberyard(Grid2<char> state, Point2 p)
    {
        var lumberyardCount = 0;
        var treeCount = 0;

        foreach (var n in p.GetNeighbors(true).Where(IsInBounds))
        {
            if (state[n] == '#')
                lumberyardCount++;
            else if (state[n] == '|')
                treeCount++;

            if (lumberyardCount > 0 && treeCount > 0)
                return '#';
        }

        return '.';
    }

    private static (int Tree, int Lumberyard) Count(Grid2<char> state)
    {
        int tree = 0, lumberyard = 0;
        foreach (var (_, c) in state)
            switch (c)
            {
                case '.':
                    break;
                case '|':
                    tree++;
                    break;
                case '#':
                    lumberyard++;
                    break;
            }

        return (tree, lumberyard);
    }

    private static bool IsInBounds(Point2 p) => p.X >= 0 && p.Y >= 0 && p.X < 50 && p.Y < 50;
}

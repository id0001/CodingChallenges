using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;
using Spectre.Console;
using System.Collections.Immutable;

namespace AdventOfCode2018.Challenges;

[Challenge(15)]
public class Challenge15
{
    [Part(1, "201123")]
    public string Part1(string input)
    {
        var (terrain, units) = Parse(input.ToGrid());

        // Update elves
        units = units.Select(kv => kv.Value is Elf elf ? new KeyValuePair<Point2, Unit>(kv.Key, elf with { Ap = 3 }) : kv).ToImmutableDictionary();
        Run(terrain, units, out var score);
        return score.ToString();
    }

    [Part(2, "54188")]
    public string Part2(string input)
    {
        var (terrain, units) = Parse(input.ToGrid());

        for(var ap = 4; ap < 200; ap++)
        {
            var updatedUnits = units.Select(kv => kv.Value is Elf elf ? new KeyValuePair<Point2, Unit>(kv.Key, elf with { Ap = ap }) : kv).ToImmutableDictionary();
            if (Run(terrain, updatedUnits, out var score))
                return score.ToString();
        }

        throw new InvalidOperationException();
    }

    private static bool Run(Grid2<char> terrain, ImmutableDictionary<Point2, Unit> units, out long score)
    {
        var elfCount = units.Count(kv => kv.Value is Elf);
        score = 0;
        var round = 0;
        while(true)
        {
            foreach(var key in units.Keys.OrderBy(k => k.Y).ThenBy(k => k.X).ToArray())
            {
                if (!units.ContainsKey(key))
                    continue;

                if(!units.Values.OfType<Elf>().Any() || !units.Values.OfType<Goblin>().Any())
                {
                    score = units.Values.Sum(x => x.Hp) * round;
                    return units.All(x => x.Value is Elf) && elfCount == units.Count;
                }

                switch (units[key])
                {
                    case Elf:
                        units = ExecuteTurn(terrain, units, key, GetPositions<Goblin>(units));
                        break;
                    case Goblin:
                        units = ExecuteTurn(terrain, units, key, GetPositions<Elf>(units));
                        break;
                }
            }

            round++;
        }
    }

    private static ImmutableDictionary<Point2, Unit> ExecuteTurn(Grid2<char> terrain, ImmutableDictionary<Point2, Unit> units, Point2 currentPosition, ISet<Point2> targets)
    {
        // Move or stay
        var nextPosition = NextPosition(terrain, units, currentPosition, targets);
        if(currentPosition != nextPosition)
        {
            var unit = units[currentPosition];
            units = units.Remove(currentPosition).Add(nextPosition, unit);
            currentPosition = nextPosition;
        }

        // Attack
        var targetsInRange = GetTargetsInRange(currentPosition, targets).ToList();
        if (targetsInRange.Count == 0)
            return units;

        var target = targetsInRange.MinBy(t => units[t].Hp);
        units = units.SetItem(target, units[target] with { Hp = units[target].Hp - units[currentPosition].Ap });
        if (units[target].Hp <= 0)
            units = units.Remove(target);

        return units;
    }

    private static Point2 NextPosition(Grid2<char> terrain, ImmutableDictionary<Point2, Unit> units, Point2 currentPosition, ISet<Point2> targets)
    {
        if(GetTargetsInRange(currentPosition,targets).Any())
            return currentPosition;

        var occupied = units.Keys.ToHashSet();
        var spacesToCheck = targets.SelectMany(t => GetOpenSpacesAroundTarget(t, terrain, occupied)).ToList();

        var bfs = Graph.Implicit<Point2>(p => GetAdjacent(p, terrain, occupied)).Bfs();

        var reachable = bfs
            .Distances(currentPosition)
            .IntersectBy(spacesToCheck, s => s.Key)
            .OrderBy(kv => kv.Value)
            .ThenBy(kv => kv.Key.Y)
            .ThenBy(kv => kv.Key.X)
            .Select(kv => (Point2?)kv.Key)
            .FirstOrDefault();

        if (!reachable.HasValue)
            return currentPosition;

        return bfs.ShortestPath(currentPosition, reachable.Value).Second();
    }

    private static IEnumerable<Point2> GetTargetsInRange(Point2 current, ISet<Point2> targets) 
        => current.GetNeighbors().Where(targets.Contains);

    private static IEnumerable<Point2> GetOpenSpacesAroundTarget(Point2 target, Grid2<char> terrain, ISet<Point2> occupied)
        => target.GetNeighbors().Where(n => !occupied.Contains(n) && terrain[n] == '.').OrderBy(n => n.Y)
            .ThenBy(n => n.X);

    private static IEnumerable<(Point2, Point2)> GetAdjacent(Point2 current, Grid2<char> terrain, ISet<Point2> units)
        => GetOpenSpacesAroundTarget(current, terrain, units).Select(p => (current,p));

    private static HashSet<Point2> GetPositions<T>(IDictionary<Point2, Unit> units)
        where T : Unit
        => [.. units.Where(kv => kv.Value is T).Select(x => x.Key)];

    private static (Grid2<char> Terrain, ImmutableDictionary<Point2, Unit> Units) Parse(Grid2<char> grid)
    {
        var units = new Dictionary<Point2, Unit>();
        foreach(var (p,c) in grid)
        {
            if(c == 'G')
            {
                units.Add(p, new Goblin());
                grid[p] = '.';
            }

            if(c == 'E')
            {
                units.Add(p, new Elf());
                grid[p] = '.';
            }
        }

        return (grid, units.ToImmutableDictionary());
    }

    private record Unit(long Hp, long Ap);
    private record Elf() : Unit(200,3);
    private record Goblin() : Unit(200,3);
}

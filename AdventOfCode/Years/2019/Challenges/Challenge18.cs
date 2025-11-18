using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;
using System.Collections;
using System.Numerics;

namespace AdventOfCode2019.Challenges;

[Challenge(18)]
public class Challenge18
{
    private const int AllKeyMask = 0b0011_1111_1111_1111_1111_1111_1111;

    [Part(1, "3146")]
    public string Part1(string input)
    {
        var grid = input.ToGrid();
        var state = new State(grid.Single(kv => kv.Value == '@').Key, 0);

        var path = Graph.Implicit<State>(c => GetAdjacent(grid, c)).Bfs().ShortestPath(state, s => (s.ObtainedKeys & AllKeyMask) == AllKeyMask);

        return (path.Length - 1).ToString();
    }

    // [Part(2, "2194")]
    //public string Part2(string input)
    //{
    //    throw new NoResultException();
    //}

    private static IEnumerable<Edge<State>> GetAdjacent(Grid2<char> map, State current)
    {
        foreach(var neighbor in current.Position.GetNeighbors())
        {
            var c = map[neighbor];
            var keys = current.ObtainedKeys;

            // Is wall?
            if (c == '#')
                continue;

            // Is locked door?
            if (char.IsUpper(c) && !keys.IsBitSet(c - 'A'))
                continue;


            if (char.IsLower(c))
                keys = keys.SetBit(c - 'a', true);

            yield return new Edge<State>(current, new State(neighbor, keys));    
        }
    }

    private record State(Point2 Position, int ObtainedKeys);
}

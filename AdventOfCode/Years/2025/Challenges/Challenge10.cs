using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Immutable;
using CodingChallenge.Utilities.Extensions;
using Microsoft.Z3;
using System.Collections.Immutable;
using System.Numerics;
using System.Xml.Serialization;

namespace AdventOfCode2025.Challenges;

[Challenge(10)]
public class Challenge10
{
    [Part(1, "415")]
    public string Part1(string input)
    {
        int sum = 0;
        foreach (var line in input.Lines())
        {
            var split = line.SplitBy(" ");

            var lights = split[0][1..^1];
            var schematics = split[1..^1].Select(s => s[1..^1].SplitBy<int>(",").ToArray()).ToList();

            var start = CreateLights(new string('.', lights.Length));
            var end = CreateLights(lights);

            var path = Graph.Implicit<ImmutableBitVector32>(c => GetAdjacentLights(schematics, c)).Bfs().ShortestPath(start, end);
            sum += path.Length - 1;
        }

        return sum.ToString();
    }

    [Part(2, "16663")]
    public string Part2(string input)
    {
        /*
         * Given:
         * (3) (1,3) (2) (2,3) (0,2) (0,1)
         * {3,5,4,7}
         * 
         * 0 0 0 0 1 1 | 3
         * 0 1 0 0 0 1 | 5
         * 0 0 1 1 1 0 | 4
         * 1 1 0 1 0 0 | 7
         * a b c d e f | s
         * 
         * 0a + 0b + 0c + 0d + 1e + 1f = 3
         * 0a + 1b + 0c + 0d + 0e + 1f = 5
         * 0a + 0b + 1c + 1d + 1e + 0f = 4
         * 1a + 1b + 0c + 1d + 0e + 0f = 7
         * a  + b  + c  + d  + e  + f  = s (minimize s)
        */
        using var ctx = new Context();

        int result = 0;
        foreach (var line in input.Lines())
        {
            var split = line.SplitBy(" ");
            var joltage = split[^1][1..^1].SplitBy<int>(",").ToArray();
            var schematics = split[1..^1].Select(s => s[1..^1].SplitBy<int>(",").ToArray()).ToList();

            Solver s = ctx.MkSolver();

            // These are the times a specific index is multiplied
            IntExpr[] xExpressions = new IntExpr[schematics.Count];
            for (var i = 0; i < schematics.Count; i++)
            {
                xExpressions[i] = ctx.MkIntConst("x" + i);
                s.Assert(ctx.MkGe(xExpressions[i], ctx.MkInt(0)));
            }

            // All the possible additions per index
            int[,] groups = new int[schematics.Count, joltage.Length];
            for (var y = 0; y < schematics.Count; y++)
            {
                for (var x = 0; x < schematics[y].Length; x++)
                    groups[y, schematics[y][x]] = 1;
            }

            for (int i = 0; i < joltage.Length; i++)
            {
                // Create the equasion for each index and assert that its equal to the target
                ArithExpr sum = ctx.MkInt(0);
                for (int j = 0; j < schematics.Count; j++)
                    sum = ctx.MkAdd(sum, ctx.MkMul(ctx.MkInt(groups[j, i]), xExpressions[j]));

                s.Assert(ctx.MkEq(sum, ctx.MkInt(joltage[i])));
            }

            // Optimize the solve.
            Optimize opt = ctx.MkOptimize();
            foreach (var c in s.Assertions)
                opt.Add(c);

            // Sum the total xn expressions
            ArithExpr total = ctx.MkInt(0);
            foreach (var xi in xExpressions)
                total = ctx.MkAdd(total, xi);

            Optimize.Handle h = opt.MkMinimize(total);
            if (opt.Check() == Status.SATISFIABLE)
            {
                Model m = opt.Model;
                result += ((IntNum)m.Evaluate(total)).Int;
            }
        }

        return result.ToString();
    }

    private IEnumerable<(ImmutableBitVector32, ImmutableBitVector32)> GetAdjacentLights(List<int[]> schematics, ImmutableBitVector32 current)
    {
        foreach (var schematic in schematics)
        {
            var newLights = current;
            for (var i = 0; i < schematic.Length; i++)
                newLights = newLights.Set(schematic[i], newLights.Get(schematic[i]) == 1 ? 0 : 1);

            yield return (current, newLights);
        }
    }

    private ImmutableBitVector32 CreateLights(string lights)
    {
        var bits = ImmutableBitVector32.Create(1, lights.Length);
        for (var i = 0; i < lights.Length; i++)
            bits = bits.Set(i, lights[i] == '#' ? 1 : 0);

        return bits;
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Immutable;

namespace AdventOfCode2016.Challenges;

[Challenge(11)]
public class Challenge11
{
    [Part(1, "31")]
    public string Part1(string input)
    {
        var bits = new ImmutableBitVector32();
        for (var i = 0; i < 10; i++)
            bits = bits.AddSection(0b11);

        bits = bits.Set(0, 0); // TM
        bits = bits.Set(1, 1); // PM
        bits = bits.Set(2, 1); // SM
        bits = bits.Set(3, 2); // PRM
        bits = bits.Set(4, 2); // RM

        bits = bits.Set(5, 0); // TG
        bits = bits.Set(6, 0); // PG
        bits = bits.Set(7, 0); // SG
        bits = bits.Set(8, 2); // PRG
        bits = bits.Set(9, 2); // RG

        var state = new State(0, bits, 5);
        var graph = Graph.Implicit<State>(GetAjacent);

        var path = graph.Bfs().ShortestPath(state, s => s.IsFinished);
        return (path.Length - 1).ToString();
    }

    [Part(2, "55")]
    public string Part2(string input)
    {
        var bits = new ImmutableBitVector32();
        for (var i = 0; i < 14; i++)
            bits = bits.AddSection(0b11);

        bits = bits.Set(0, 0); // TM
        bits = bits.Set(1, 1); // PM
        bits = bits.Set(2, 1); // SM
        bits = bits.Set(3, 2); // PRM
        bits = bits.Set(4, 2); // RM
        bits = bits.Set(5, 0); // EM
        bits = bits.Set(6, 0); // DM

        bits = bits.Set(7, 0); // TG
        bits = bits.Set(8, 0); // PG
        bits = bits.Set(9, 0); // SG
        bits = bits.Set(10, 2); // PRG
        bits = bits.Set(11, 2); // RG
        bits = bits.Set(12, 0); // EG
        bits = bits.Set(13, 0); // DG


        var state = new State(0, bits, 7);
        var graph = Graph.Implicit<State>(GetAjacent);

        var path = graph.Bfs().ShortestPath(state, s => s.IsFinished);
        return (path.Length - 1).ToString();
    }

    private static IEnumerable<Edge<State>> GetAjacent(State current)
    {
        // Go up
        if (current.CurrentLevel < 3)
            foreach (var next in current.GetNext(1))
                yield return new Edge<State>(current, next);

        // Go down
        if (current.CurrentLevel > 0)
            foreach (var next in current.GetNext(-1))
                yield return new Edge<State>(current,next);
    }

    private sealed record State(int CurrentLevel, ImmutableBitVector32 Bits, int PairCount)
    {
        public bool IsFinished => CurrentLevel == 3 && Enumerable.Range(0, PairCount * 2).All(i => Bits.Get(i) == 3);

        public bool IsValid => Enumerable
            .Range(0, 4)
            .All(level =>
            {
                if (!GetIndicesOnLevel(level).Any(i => i >= PairCount))
                    return true;

                return GetIndicesOnLevel(level).Where(i => i < PairCount).All(i => Bits.Get(i + PairCount) == level);
            });

        public bool Equals(State? other)
        {
            if (other is null)
                return false;

            return GetEquivalenceArray().SequenceEqual(other.GetEquivalenceArray());
        }

        public IEnumerable<State> GetNext(int moveAmount)
        {
            var newLevel = CurrentLevel + moveAmount;

            // Doubles
            foreach (var combination in GetIndicesOnLevel(CurrentLevel).Combinations(2))
            {
                var bits = Bits
                    .Increment(combination[0], moveAmount)
                    .Increment(combination[1], moveAmount);

                var next = new State(newLevel, bits, PairCount);

                if (next.IsValid)
                    yield return next;
            }

            // Singles
            foreach (var index in GetIndicesOnLevel(CurrentLevel))
            {
                var next = new State(newLevel, Bits.Increment(index, moveAmount), PairCount);
                if (next.IsValid)
                    yield return next;
            }
        }

        public override int GetHashCode()
        {
            var hc = new HashCode();
            foreach (var v in GetEquivalenceArray())
                hc.Add(v);

            return hc.ToHashCode();
        }

        private IEnumerable<int> GetIndicesOnLevel(int level) => Enumerable
            .Range(0, PairCount * 2)
            .Where(i => Bits.Get(i) == level);

        private int[] GetEquivalenceArray() => Enumerable
            .Range(0, 4)
            .Select(level =>
            {
                var indices = GetIndicesOnLevel(level).ToArray();
                var generators = indices.Where(i => i >= PairCount).ToArray();
                var chips = indices.Where(i => i < PairCount).ToArray();
                var pairs = chips.Count(i => Bits.Get(i + PairCount) == level);
                return chips.Length | (generators.Length << 8) | (pairs << 16) | (CurrentLevel << 24);
            })
            .ToArray();
    }
}

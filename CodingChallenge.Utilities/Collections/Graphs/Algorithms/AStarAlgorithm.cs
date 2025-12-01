namespace CodingChallenge.Utilities.Collections.Graphs.Algorithms
{
    public sealed record AStarAlgorithm<TVertex>(Func<TVertex, IEnumerable<(TVertex Source, TVertex Target, int Weight)>> OutEdges, Func<TVertex, int> Heuristic)
        where TVertex : notnull, IEquatable<TVertex>;
}

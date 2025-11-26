namespace CodingChallenge.Utilities.Collections.Graphs.Algorithms
{
    public sealed record AStarAlgorithm<TVertex, TEdge>(Func<TVertex, IEnumerable<TEdge>> OutEdges, Func<TVertex, int> Heuristic)
        where TVertex : notnull, IEquatable<TVertex>
        where TEdge : WeightedEdge<TVertex, int>;
}

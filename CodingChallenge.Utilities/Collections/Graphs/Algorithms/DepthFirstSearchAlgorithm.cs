namespace CodingChallenge.Utilities.Collections.Graphs.Algorithms
{
    public sealed record DepthFirstSearchAlgorithm<TVertex>(Func<TVertex, IEnumerable<(TVertex Source, TVertex Target)>> OutEdges)
        where TVertex : notnull, IEquatable<TVertex>;

    public sealed record WeightedDepthFirstSearchAlgorithm<TVertex, TWeight>(Func<TVertex, IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)>> OutEdges)
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull;
}

namespace CodingChallenge.Utilities.Collections.Graphs.Algorithms
{
    public sealed record DepthFirstSearchAlgorithm<TVertex, TEdge>(Func<TVertex, IEnumerable<TEdge>> OutEdges)
        where TVertex : notnull, IEquatable<TVertex>
        where TEdge : notnull, Edge<TVertex>;
}

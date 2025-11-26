namespace CodingChallenge.Utilities.Collections.Graphs.Algorithms
{
    public record BreadthFirstSearchAlgorithm<TVertex, TEdge>(Func<TVertex, IEnumerable<TEdge>> OutEdges)
        where TVertex : notnull, IEquatable<TVertex>
        where TEdge : notnull, Edge<TVertex>;
}

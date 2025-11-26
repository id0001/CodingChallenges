namespace CodingChallenge.Utilities.Collections.Graphs.Algorithms
{
    public record FloydWarshallAlgorithm<TVertex, TEdge>(IList<TVertex> Vertices, IList<TEdge> Edges)
        where TVertex : notnull, IEquatable<TVertex>
        where TEdge : notnull, WeightedEdge<TVertex, int>;
}

namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IVertexReversible<TSelf>
    {
        TSelf Reverse();
    }

    public interface IEdge<TVertex>
        where TVertex : notnull, IEquatable<TVertex>
    {
        TVertex Source { get; init; }

        TVertex Target { get; init; }
    }

    public record Edge<TVertex>(TVertex Source, TVertex Target) : IEdge<TVertex>, IVertexReversible<Edge<TVertex>>
        where TVertex : notnull, IEquatable<TVertex>
    {
        public Edge<TVertex> Reverse() => new(Target, Source);
    }

    public record WeightedEdge<TVertex, TWeight>(TVertex Source, TVertex Target, TWeight Weight) : IEdge<TVertex>, IVertexReversible<WeightedEdge<TVertex, TWeight>>
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull
    {
        WeightedEdge<TVertex, TWeight> IVertexReversible<WeightedEdge<TVertex, TWeight>>.Reverse() => new(Target, Source, Weight);
    }
}

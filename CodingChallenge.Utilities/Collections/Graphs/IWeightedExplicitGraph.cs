namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IWeightedExplicitGraph<TVertex, TWeight> : IWeightedImplicitGraph<TVertex, TWeight>
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull
    {
        IReadOnlySet<TVertex> Vertices { get; }

        bool ContainsVertex(TVertex vertex);

        int OutDegrees(TVertex source);

        int InDegrees(TVertex target);

        IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)> InEdges(TVertex target);
    }
}

namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IExplicitGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TVertex : notnull, IEquatable<TVertex>
        where TEdge : Edge<TVertex>
    {
        bool ContainsVertex(TVertex vertex);

        IReadOnlySet<TVertex> Vertices { get; }

        int OutDegree(TVertex source);

        int InDegree(TVertex target);
    }
}

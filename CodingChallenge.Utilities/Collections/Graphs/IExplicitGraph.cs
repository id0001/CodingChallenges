namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IExplicitGraph<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        bool ContainsVertex(TVertex vertex);

        IReadOnlySet<TVertex> Vertices { get; }
    }
}

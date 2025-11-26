namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IImplicitGraph<TVertex, TEdge>
        where TVertex : notnull, IEquatable<TVertex>
        where TEdge : notnull, Edge<TVertex>
    {
        IEnumerable<TEdge> OutEdges(TVertex source);
        IEnumerable<TEdge> InEdges(TVertex target);
    }
}

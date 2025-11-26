namespace CodingChallenge.Utilities.Collections.Graphs
{
    public class ImplicitSearchGraph<TVertex, TEdge>(Func<TVertex, IEnumerable<TEdge>> getAjacent) : IImplicitGraph<TVertex, TEdge>
        where TVertex : notnull, IEquatable<TVertex>
        where TEdge : Edge<TVertex>
    {
        public IEnumerable<TEdge> InEdges(TVertex target) => Enumerable.Empty<TEdge>();

        public IEnumerable<TEdge> OutEdges(TVertex source) => getAjacent(source);
    }
}

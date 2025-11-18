namespace CodingChallenge.Utilities.Collections.Graphs
{
    public class ImplicitGraph<TVertex, TEdge>(Func<TVertex, IEnumerable<TEdge>> getAjacent) : ISearchableGraph<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly Func<TVertex, IEnumerable<TEdge>> _getAjacent = getAjacent;

        public int OutDegree(TVertex vertex) => _getAjacent(vertex).Count();

        public IEnumerable<TEdge> OutEdges(TVertex vertex) => _getAjacent(vertex);
    }
}

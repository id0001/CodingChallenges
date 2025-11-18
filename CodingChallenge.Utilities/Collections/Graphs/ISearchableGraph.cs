namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface ISearchableGraph<TVertex, TEdge>
    {
        IEnumerable<TEdge> OutEdges(TVertex current);

        int OutDegree(TVertex vertex);
    }
}

namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IMutableGraph<TVertex>
        where TVertex : notnull, IEquatable<TVertex>
    {
        bool AddVertex(TVertex vertex);

        bool RemoveVertex(TVertex vertex);

        bool AddEdge(TVertex source, TVertex target);

        bool RemoveEdge(TVertex source, TVertex target);
    }
}

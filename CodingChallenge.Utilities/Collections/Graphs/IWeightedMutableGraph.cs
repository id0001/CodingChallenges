namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IWeightedMutableGraph<TVertex, TWeight>
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull
    {
        bool AddVertex(TVertex vertex);

        bool RemoveVertex(TVertex vertex);

        bool AddEdge(TVertex source, TVertex target, TWeight weight);

        bool RemoveEdge(TVertex source, TVertex target);
    }
}

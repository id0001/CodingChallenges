namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IWeightedImplicitGraph<TVertex, TWeight>
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull
    {
        IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)> OutEdges(TVertex source);
    }
}

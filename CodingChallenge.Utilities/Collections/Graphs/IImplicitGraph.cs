namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IImplicitGraph<TVertex>
        where TVertex : notnull, IEquatable<TVertex>
    {
        IEnumerable<(TVertex Source, TVertex Target)> OutEdges(TVertex source);
    }
}

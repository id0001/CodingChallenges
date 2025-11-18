namespace CodingChallenge.Utilities.Collections.Graphs
{
    public interface IEdge<TVertex>
        where TVertex : notnull
    {
        TVertex Source { get; }
        TVertex Target { get; }
    }

    public record Edge<TVertex>(TVertex Source, TVertex Target) : IEdge<TVertex>
        where TVertex : notnull;

    public record WeightedEdge<TVertex, TWeight>(TVertex Source, TVertex Target, TWeight Weight) :IEdge<TVertex>
        where TVertex : notnull
        where TWeight : notnull;
}

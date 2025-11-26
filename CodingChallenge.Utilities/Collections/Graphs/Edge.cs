namespace CodingChallenge.Utilities.Collections.Graphs
{
    public record Edge<TVertex>(TVertex Source, TVertex Target)
        where TVertex : notnull, IEquatable<TVertex>;

    public record WeightedEdge<TVertex, TWeight>(TVertex Source, TVertex Target, TWeight Weight) : Edge<TVertex>(Source, Target)
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull;
}

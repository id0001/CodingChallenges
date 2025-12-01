namespace CodingChallenge.Utilities.Collections.Graphs.Algorithms
{
    public record FloydWarshallAlgorithm<TVertex>(IList<TVertex> Vertices, IList<(TVertex Source, TVertex Target, int Weight)> Edges)
        where TVertex : notnull, IEquatable<TVertex>;
}

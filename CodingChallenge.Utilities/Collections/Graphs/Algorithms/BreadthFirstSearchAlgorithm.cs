namespace CodingChallenge.Utilities.Collections.Graphs.Algorithms
{
    public record BreadthFirstSearchAlgorithm<TVertex>(Func<TVertex, IEnumerable<(TVertex Source, TVertex Target)>> OutEdges)
        where TVertex : notnull, IEquatable<TVertex>;
}

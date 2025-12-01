namespace CodingChallenge.Utilities.Collections.Graphs
{
    public class ImplicitGraph<TVertex>(Func<TVertex, IEnumerable<(TVertex Source, TVertex Target)>> outEdges) : IImplicitGraph<TVertex>
        where TVertex : notnull, IEquatable<TVertex>
    {
        public IEnumerable<(TVertex Source, TVertex Target)> OutEdges(TVertex source) => outEdges(source);
    }
}

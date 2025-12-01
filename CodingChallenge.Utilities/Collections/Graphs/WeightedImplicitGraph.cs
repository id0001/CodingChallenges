namespace CodingChallenge.Utilities.Collections.Graphs
{
    public class WeightedImplicitGraph<TVertex, TWeight>(Func<TVertex, IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)>> outEdges) : IWeightedImplicitGraph<TVertex, TWeight>, IImplicitGraph<TVertex>
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull
    {
        public IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)> OutEdges(TVertex source) => outEdges(source);

        IEnumerable<(TVertex Source, TVertex Target)> IImplicitGraph<TVertex>.OutEdges(TVertex source) => OutEdges(source).Select(edge => (edge.Source, edge.Target));
    }
}

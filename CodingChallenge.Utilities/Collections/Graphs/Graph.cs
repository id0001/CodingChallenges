namespace CodingChallenge.Utilities.Collections.Graphs
{
    public static class Graph
    {
        public static ImplicitGraph<TVertex, Edge<TVertex>> Implicit<TVertex>(Func<TVertex, IEnumerable<Edge<TVertex>>> getAdjacent)
            where TVertex : notnull
            => new ImplicitGraph<TVertex, Edge<TVertex>>(getAdjacent);

        public static ImplicitGraph<TVertex, WeightedEdge<TVertex, TWeight>> ImplicitWeighted<TVertex, TWeight>(Func<TVertex, IEnumerable<WeightedEdge<TVertex, TWeight>>> getAdjacent)
            where TVertex : notnull
            where TWeight : notnull
            => new ImplicitGraph<TVertex, WeightedEdge<TVertex, TWeight>>(getAdjacent);
    }
}

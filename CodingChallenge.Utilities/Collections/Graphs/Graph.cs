namespace CodingChallenge.Utilities.Collections.Graphs
{
    public static class Graph
    {
        public static ImplicitSearchGraph<TVertex, Edge<TVertex>> Implicit<TVertex>(Func<TVertex, IEnumerable<Edge<TVertex>>> getAdjacent)
            where TVertex : notnull, IEquatable<TVertex>
            => new ImplicitSearchGraph<TVertex, Edge<TVertex>>(getAdjacent);

        public static ImplicitSearchGraph<TVertex, WeightedEdge<TVertex, TWeight>> ImplicitWeighted<TVertex, TWeight>(Func<TVertex, IEnumerable<WeightedEdge<TVertex, TWeight>>> getAdjacent)
            where TVertex : notnull, IEquatable<TVertex>
            where TWeight : notnull
            => new ImplicitSearchGraph<TVertex, WeightedEdge<TVertex, TWeight>>(getAdjacent);
    }
}

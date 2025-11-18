using CodingChallenge.Utilities.Collections.Graphs;

namespace CodingChallenge.Utilities.Extensions
{
    public static class GraphExtensions
    {
        extension<TVertex, TEdge>(ISearchableGraph<TVertex, TEdge> source)
            where TVertex : notnull
            where TEdge : notnull, WeightedEdge<TVertex, int>
        {
            public AStar<TVertex, TEdge> AStar(Func<TVertex, int> heuristic)
                => new AStar<TVertex, TEdge>(source, heuristic);

            public AStar<TVertex, TEdge> Dijkstra()
                => new AStar<TVertex, TEdge>(source, _ => 0);
        }

        extension<TVertex, TEdge>(ISearchableGraph<TVertex, TEdge> source)
            where TVertex : notnull
            where TEdge : notnull, Edge<TVertex>
        {
            public Bfs<TVertex, TEdge> Bfs()
            => new Bfs<TVertex, TEdge>(source);

            public Dfs<TVertex, TEdge> Dfs()
                => new Dfs<TVertex, TEdge>(source);
        }
    }
}

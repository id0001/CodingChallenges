using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Graphs.Algorithms;

namespace CodingChallenge.Utilities.Extensions
{
    public static class GraphExtensions
    {
        extension<TVertex, TEdge>(IImplicitGraph<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
            where TEdge : notnull, WeightedEdge<TVertex, int>
        {
            public AStarAlgorithm<TVertex, TEdge> AStar(Func<TVertex, int> heuristic)
                => new AStarAlgorithm<TVertex, TEdge>(source.OutEdges, heuristic);

            public AStarAlgorithm<TVertex, TEdge> Dijkstra()
                => new AStarAlgorithm<TVertex, TEdge>(source.OutEdges, _ => 0);
        }

        extension<TVertex, TEdge>(IImplicitGraph<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
            where TEdge : notnull, Edge<TVertex>
        {
            public BreadthFirstSearchAlgorithm<TVertex, TEdge> Bfs()
            => new BreadthFirstSearchAlgorithm<TVertex, TEdge>(source.OutEdges);

            public DepthFirstSearchAlgorithm<TVertex, TEdge> Dfs()
                => new DepthFirstSearchAlgorithm<TVertex, TEdge>(source.OutEdges);
        }

        extension<TVertex, TEdge>(IExplicitGraph<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
            where TEdge : notnull, WeightedEdge<TVertex, int>
        {
            public FloydWarshallAlgorithm<TVertex, TEdge> FloydWarshall()
            {
                ArgumentNullException.ThrowIfNull(source);

                var vertices = source.Vertices.ToList();
                var edges = vertices.SelectMany(source.OutEdges).ToList();
                return new FloydWarshallAlgorithm<TVertex, TEdge>(vertices, edges);
            }
        }
    }
}

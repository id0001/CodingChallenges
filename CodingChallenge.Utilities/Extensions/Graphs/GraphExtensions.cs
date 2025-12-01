using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Graphs.Algorithms;

namespace CodingChallenge.Utilities.Extensions
{
    public static class GraphExtensions
    {
        extension<TVertex>(IWeightedImplicitGraph<TVertex, int> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public AStarAlgorithm<TVertex> AStar(Func<TVertex, int> heuristic)
                => new AStarAlgorithm<TVertex>(source.OutEdges, heuristic);

            public AStarAlgorithm<TVertex> Dijkstra()
                => new AStarAlgorithm<TVertex>(source.OutEdges, _ => 0);
        }

        extension<TVertex>(IImplicitGraph<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public BreadthFirstSearchAlgorithm<TVertex> Bfs()
            => new BreadthFirstSearchAlgorithm<TVertex>(source.OutEdges);

            public DepthFirstSearchAlgorithm<TVertex> Dfs()
                => new DepthFirstSearchAlgorithm<TVertex>(source.OutEdges);
        }

        extension<TVertex>(IWeightedExplicitGraph<TVertex, int> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public FloydWarshallAlgorithm<TVertex> FloydWarshall()
            {
                ArgumentNullException.ThrowIfNull(source);

                var vertices = source.Vertices.ToList();
                var edges = vertices.SelectMany(source.OutEdges).ToList();
                return new FloydWarshallAlgorithm<TVertex>(vertices, edges);
            }
        }
    }
}

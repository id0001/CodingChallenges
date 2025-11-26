using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Graphs.Algorithms;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class FloydWarshallExtensions
    {
        extension<TVertex, TEdge>(FloydWarshallAlgorithm<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
            where TEdge : notnull, WeightedEdge<TVertex, int>
        {
            public Dictionary<(TVertex, TVertex), int> Distances()
            {
                ArgumentNullException.ThrowIfNull(source);

                var vertices = source.Vertices;
                var edges = source.Edges;

                var matrix = new int?[vertices.Count, vertices.Count];

                for (var i = 0; i < vertices.Count; i++)
                    matrix[i, i] = 0;

                foreach (var edge in edges)
                    matrix[vertices.IndexOf(edge.Source), vertices.IndexOf(edge.Target)] = edge.Weight;

                for (var k = 0; k < vertices.Count; k++)
                    for (var i = 0; i < vertices.Count; i++)
                        for (var j = 0; j < vertices.Count; j++)
                            if (matrix[i, k].HasValue && matrix[k, j].HasValue &&
                                (!matrix[i, j].HasValue || matrix[i, j] > matrix[i, k] + matrix[k, j]))
                                matrix[i, j] = matrix[i, k] + matrix[k, j];

                var distances = new Dictionary<(TVertex, TVertex), int>();
                for (var y = 0; y < vertices.Count; y++)
                    for (var x = 0; x < vertices.Count; x++)
                        if (matrix[x, y].HasValue)
                            distances.Add((vertices[x], vertices[y]), matrix[x, y]!.Value);

                return distances;
            }
        }
    }
}

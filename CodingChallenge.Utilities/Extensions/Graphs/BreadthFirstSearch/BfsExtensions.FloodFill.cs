using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Graphs.Algorithms;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class BfsExtensions
    {
        extension<TVertex, TEdge>(BreadthFirstSearchAlgorithm<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
            where TEdge : notnull, Edge<TVertex>
        {
            public IEnumerable<(TVertex Value, int Distance)> FloodFill(TVertex start, int maxSteps = int.MaxValue)
            {
                ArgumentNullException.ThrowIfNull(source);

                Queue<TVertex> queue = new([start]);
                Dictionary<TVertex, int> visited = new() { [start] = 0 };

                while (queue.Count > 0)
                {
                    var currentVertex = queue.Dequeue();
                    int distance = visited[currentVertex];

                    yield return (Value: currentVertex, Distance: distance);
                    if (distance == maxSteps)
                        continue;

                    foreach (var nextEdge in source.OutEdges(currentVertex))
                    {
                        if (visited.ContainsKey(nextEdge.Target))
                            continue;

                        visited.Add(nextEdge.Target, distance + 1);
                        queue.Enqueue(nextEdge.Target);
                    }
                }
            }
        }
    }
}

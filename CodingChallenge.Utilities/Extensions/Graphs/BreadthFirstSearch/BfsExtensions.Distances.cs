using CodingChallenge.Utilities.Collections.Graphs.Algorithms;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class BfsExtensions
    {
        extension<TVertex>(BreadthFirstSearchAlgorithm<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public Dictionary<TVertex, int> Distances(TVertex start)
            {
                ArgumentNullException.ThrowIfNull(source);

                Queue<TVertex> queue = new([start]);
                Dictionary<TVertex, int> visited = new() { [start] = 0 };

                while (queue.Count > 0)
                {
                    var currentVertex = queue.Dequeue();
                    int distance = visited[currentVertex];

                    foreach (var nextEdge in source.OutEdges(currentVertex))
                    {
                        if (visited.ContainsKey(nextEdge.Target))
                            continue;

                        visited.Add(nextEdge.Target, distance + 1);
                        queue.Enqueue(nextEdge.Target);
                    }
                }

                return visited;
            }
        }
    }
}

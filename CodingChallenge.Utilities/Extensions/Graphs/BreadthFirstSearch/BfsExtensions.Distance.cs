using CodingChallenge.Utilities.Collections.Graphs.Algorithms;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class BfsExtensions
    {
        extension<TVertex>(BreadthFirstSearchAlgorithm<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public int Distance(TVertex from, TVertex to)
                => Distance(source, from, c => c.Equals(to));

            public int Distance(TVertex from, Func<TVertex, bool> isFinished)
            {
                Queue<TVertex> queue = new([from]);
                Dictionary<TVertex, int> visited = new() { [from] = 0 };

                while (queue.Count > 0)
                {
                    var currentVertex = queue.Dequeue();
                    int distance = visited[currentVertex];

                    if (isFinished(currentVertex))
                        return distance;

                    foreach (var nextEdge in source.OutEdges(currentVertex))
                    {
                        if (visited.ContainsKey(nextEdge.Target))
                            continue;

                        visited.Add(nextEdge.Target, distance + 1);
                        queue.Enqueue(nextEdge.Target);
                    }
                }

                return -1;
            }
        }
    }
}

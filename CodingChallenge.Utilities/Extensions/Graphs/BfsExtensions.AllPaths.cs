using CodingChallenge.Utilities.Collections.Graphs;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class BfsExtensions
    {
        extension<TVertex, TEdge>(Bfs<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
                where TEdge : notnull, Edge<TVertex>
        {
            public IEnumerable<TVertex[]> AllPaths(TVertex from, TVertex to) => AllPaths(source, from, c => c.Equals(to));

            public IEnumerable<TVertex[]> AllPaths(TVertex from, Func<TVertex, bool> isFinished)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(isFinished);

                var queue = new Queue<TVertex[]>([[from]]);

                while (queue.Count > 0)
                {
                    var currentPath = queue.Dequeue();
                    var currentVertex = currentPath[^1];

                    if (isFinished(currentVertex))
                    {
                        yield return currentPath;
                        continue;
                    }

                    foreach (var nextVertex in source.Graph.OutEdges(currentVertex))
                    {
                        if (Array.IndexOf(currentPath, nextVertex.Target) == -1)
                            queue.Enqueue([.. currentPath, nextVertex.Target]);
                    }
                }
            }
        }
    }
}

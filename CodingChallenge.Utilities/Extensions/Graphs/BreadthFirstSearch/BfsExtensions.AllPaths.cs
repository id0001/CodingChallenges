using CodingChallenge.Utilities.Collections.Graphs.Algorithms;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class BfsExtensions
    {
        extension<TVertex>(BreadthFirstSearchAlgorithm<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
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

                    foreach (var nextVertex in source.OutEdges(currentVertex))
                    {
                        if (Array.IndexOf(currentPath, nextVertex.Target) == -1)
                            queue.Enqueue([.. currentPath, nextVertex.Target]);
                    }
                }
            }
        }
    }
}

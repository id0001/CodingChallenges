using CodingChallenge.Utilities.Collections.Graphs.Algorithms;
using CodingChallenge.Utilities.Exceptions;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class BfsExtensions
    {
        extension<TVertex>(BreadthFirstSearchAlgorithm<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public TVertex[] ShortestPath(TVertex from, TVertex to)
                => ShortestPath(source, from, c => c.Equals(to));

            public TVertex[] ShortestPath(TVertex from, Func<TVertex, bool> isFinished)
            {
                if (!TryShortestPath(source, from, isFinished, out var path))
                    throw new PathNotFoundException();

                return path;
            }

            public bool TryShortestPath(TVertex from, TVertex to, out TVertex[] path)
                => TryShortestPath(source, from, c => c.Equals(to), out path);

            public bool TryShortestPath(TVertex from, Func<TVertex, bool> isFinished, out TVertex[] path)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(isFinished);

                Queue<TVertex> queue = new([from]);
                Dictionary<TVertex, TVertex> cameFrom = [];
                HashSet<TVertex> visited = [from];

                while (queue.Count > 0)
                {
                    var currentVertex = queue.Dequeue();

                    if (isFinished(currentVertex))
                    {
                        path = BuildPath(currentVertex, cameFrom);
                        return true;
                    }

                    foreach (var nextEdge in source.OutEdges(currentVertex))
                    {
                        if (visited.Contains(nextEdge.Target))
                            continue;

                        cameFrom.Add(nextEdge.Target, currentVertex);
                        visited.Add(nextEdge.Target);
                        queue.Enqueue(nextEdge.Target);
                    }
                }

                path = [];
                return false;
            }
        }
    }
}

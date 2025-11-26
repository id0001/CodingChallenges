using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Graphs.Algorithms;
using CodingChallenge.Utilities.Collections.Trees;
using CodingChallenge.Utilities.Exceptions;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class DfsExtensions
    {
        extension<TVertex, TEdge>(DepthFirstSearchAlgorithm<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
            where TEdge : notnull, Edge<TVertex>
        {
            public TVertex[] LongestPath(TVertex from, TVertex to)
                => LongestPath(source, from, c => c.Equals(to));

            public TVertex[] LongestPath(TVertex from, Func<TVertex, bool> isFinished)
            {
                if (!TryLongestPath(source, from, isFinished, out var path))
                    throw new PathNotFoundException();

                return path;
            }

            public bool TryLongestPath(TVertex from, TVertex to, out TVertex[] path)
                => TryLongestPath(source, from, c => c.Equals(to), out path);

            public bool TryLongestPath(TVertex from, Func<TVertex, bool> isFinished, out TVertex[] path)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(isFinished);

                path = [];

                Stack<GenericTree<TVertex>> stack = new([new GenericTree<TVertex>(from)]);

                int longestPathLength = -1;
                while (stack.Count > 0)
                {
                    var currentNode = stack.Pop();

                    if (isFinished(currentNode.Value))
                    {
                        if (currentNode.Depth > longestPathLength)
                        {
                            path = BuildPath(currentNode);
                            longestPathLength = currentNode.Depth;
                        }

                        continue;
                    }

                    foreach (var nextEdge in source.OutEdges(currentNode.Value))
                    {
                        var node = currentNode.AddChild(nextEdge.Target);
                        stack.Push(node);
                    }
                }

                return longestPathLength >= 0;
            }
        }
    }
}

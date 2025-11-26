using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Graphs.Algorithms;
using CodingChallenge.Utilities.Exceptions;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class AStarExtensions
    {
        extension<TVertex, TEdge>(AStarAlgorithm<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
            where TEdge : notnull, WeightedEdge<TVertex, int>
        {
            public (TVertex[] Path, int Cost) ShortestPath(TVertex from, TVertex to)
            => ShortestPath(source, from, c => c.Equals(to));

            public (TVertex[] Path, int Cost) ShortestPath(TVertex from, Func<TVertex, bool> isFinished)
            {
                if (!TryShortestPath(source, from, isFinished, out var path, out var cost))
                    throw new PathNotFoundException();

                return (path, cost);
            }

            public bool TryShortestPath(TVertex from, TVertex to, out TVertex[] path, out int cost)
                => TryShortestPath(source, from, c => c.Equals(to), out path, out cost);

            public bool TryShortestPath(TVertex from, Func<TVertex, bool> isFinished, out TVertex[] path, out int cost)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(isFinished);

                PriorityQueue<TVertex, int> queue = new([(from, 0)]);
                Dictionary<TVertex, TVertex> cameFrom = new();
                Dictionary<TVertex, int> costSoFar = new() { { from, 0 } };

                while (queue.Count > 0)
                {
                    var currentVertex = queue.Dequeue();

                    if (isFinished(currentVertex))
                    {
                        path = BuildPath(currentVertex, cameFrom);
                        cost = costSoFar[currentVertex];
                        return true;
                    }

                    foreach (var nextEdge in source.OutEdges(currentVertex))
                    {
                        var newCost = costSoFar[currentVertex] + nextEdge.Weight;
                        if (!costSoFar.ContainsKey(nextEdge.Target) || newCost < costSoFar[nextEdge.Target])
                        {
                            costSoFar[nextEdge.Target] = newCost;
                            queue.Enqueue(nextEdge.Target, newCost + source.Heuristic(nextEdge.Target));
                            cameFrom[nextEdge.Target] = currentVertex;
                        }
                    }
                }

                path = [];
                cost = -1;
                return false;
            }
        }
    }
}

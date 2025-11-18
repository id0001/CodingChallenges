using CodingChallenge.Utilities.Collections.Graphs;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class AStarExtensions
    {
        private static TVertex[] BuildPath<TVertex>(TVertex end, IDictionary<TVertex, TVertex> cameFrom)
            where TVertex : notnull
        {
            var stack = new Stack<TVertex>();
            var current = end;
            do
            {
                stack.Push(current);
            } while (cameFrom.TryGetValue(current, out current));

            return stack.ToArray();
        }
    }

    public sealed record AStar<TVertex, TEdge>(ISearchableGraph<TVertex, TEdge> Graph, Func<TVertex, int> Heuristic)
        where TVertex : notnull
        where TEdge : notnull, WeightedEdge<TVertex, int>;
}

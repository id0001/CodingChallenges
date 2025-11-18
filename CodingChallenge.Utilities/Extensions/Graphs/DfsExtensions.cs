using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Collections.Trees;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class DfsExtensions
    {
        private static TVertex[] BuildPath<TVertex>(GenericTree<TVertex> end)
           where TVertex : notnull
        {
            var stack = new Stack<TVertex>();
            var current = end;
            do
            {
                stack.Push(current.Value);
            } while ((current = current!.Parent) is { });

            return stack.ToArray();
        }
    }

    public sealed record Dfs<TVertex, TEdge>(ISearchableGraph<TVertex, TEdge> Graph)
        where TVertex : notnull
        where TEdge : notnull, Edge<TVertex>;
}

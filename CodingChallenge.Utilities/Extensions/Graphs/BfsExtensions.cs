using CodingChallenge.Utilities.Collections.Graphs;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class BfsExtensions
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

        private static List<TVertex[]> BuildPaths<TVertex>(TVertex start, TVertex end, IDictionary<TVertex, HashSet<TVertex>> previous)
            where TVertex : notnull
        {
            var paths = new List<TVertex[]>();
            var stack = new Stack<(TVertex currentNode, TVertex[] path)>();

            // Initialize the stack with the starting node and an empty path
            stack.Push((end, [end]));

            while (stack.Count > 0)
            {
                var (currentNode, path) = stack.Pop();

                if (currentNode.Equals(start))
                {
                    // Add the complete path to the results
                    paths.Add(path);
                    continue;
                }

                if (previous.TryGetValue(currentNode, out var previousNodes))
                    foreach (var prevNode in previousNodes)
                    {
                        if (Array.IndexOf(path, prevNode) >= 0)
                            continue;

                        // Create a new path including the previous node
                        var newPath = (TVertex[])[prevNode, .. path];
                        stack.Push((prevNode, newPath));
                    }
            }

            return paths;
        }
    }

    public record Bfs<TVertex, TEdge>(ISearchableGraph<TVertex, TEdge> Graph)
        where TVertex : notnull
        where TEdge : notnull, Edge<TVertex>;
}

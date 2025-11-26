namespace CodingChallenge.Utilities.Extensions
{
    public static partial class AStarExtensions
    {
        private static TVertex[] BuildPath<TVertex>(TVertex end, IDictionary<TVertex, TVertex> cameFrom)
            where TVertex : notnull, IEquatable<TVertex>
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
}

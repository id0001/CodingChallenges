namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension(Enumerable)
        {
            public static IEnumerable<T> Unfold<T>(T state, Func<T, T> generator, bool skipFirst = false)
            {
                ArgumentNullException.ThrowIfNull(generator);

                if (!skipFirst)
                    yield return state;

                for (var current = state; ;)
                {
                    var next = generator(current);
                    yield return next;
                    current = next;
                }
            }
        }
    }
}

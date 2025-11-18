namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public IEnumerable<TSource[]> Windowed(int windowSize)
            => Windowed(source, windowSize, x => x);

            public IEnumerable<TResult[]> Windowed<TResult>(int windowSize, Func<TSource, TResult> resultSelector)
                => Windowed(source, windowSize, (curr, _) => resultSelector(curr));

            public IEnumerable<TResult[]> Windowed<TResult>(int windowSize, Func<TSource, int, TResult> resultSelector)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(windowSize);
                ArgumentNullException.ThrowIfNull(resultSelector);

                using var iter = source.GetEnumerator();
                Queue<TResult> buffer = new();

                for (var i = 0; iter.MoveNext(); i++)
                {
                    buffer.Enqueue(resultSelector(iter.Current, i));
                    if (buffer.Count == windowSize)
                    {
                        yield return buffer.ToArray();
                        buffer.Dequeue();
                    }
                }
            }
        }
    }
}

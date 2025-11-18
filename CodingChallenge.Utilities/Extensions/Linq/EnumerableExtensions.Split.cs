namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public IEnumerable<IEnumerable<TSource>> Split(TSource separator)
                => Split(source, separator, null);

            public IEnumerable<IEnumerable<TSource>> Split(TSource separator, IEqualityComparer<TSource>? comparer)
            {
                comparer ??= EqualityComparer<TSource>.Default;
                return Split(source, item => comparer.Equals(item, separator), int.MaxValue, x => x);
            }
        }

        private static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc, int count, Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(separatorFunc);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
            ArgumentNullException.ThrowIfNull(resultSelector);

            List<TSource>? items = null;

            foreach (var item in source)
            {
                if (count > 0 && separatorFunc(item))
                {
                    yield return resultSelector(items ?? Enumerable.Empty<TSource>());
                    count--;
                    items = null;
                }
                else
                {
                    items ??= [];
                    items.Add(item);
                }

                if (items is { Count: > 0 })
                    yield return resultSelector(items);
            }
        }
    }
}

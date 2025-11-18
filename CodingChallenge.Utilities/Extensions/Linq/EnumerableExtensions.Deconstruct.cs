namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public void Deconstruct(out TSource? first, out IEnumerable<TSource> rest)
            {
                ArgumentNullException.ThrowIfNull(source);

                using var enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new ArgumentOutOfRangeException(nameof(source), "Collection must contain at least 1 item");

                first = enumerator.Current;

                rest = YieldRest(enumerator);
            }

            public void Deconstruct(out TSource? first, out TSource? second, out IEnumerable<TSource> rest)
            {
                ArgumentNullException.ThrowIfNull(source);

                using var enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new ArgumentOutOfRangeException(nameof(source), "Collection must contain at least 2 item");

                first = enumerator.Current;

                if (!enumerator.MoveNext())
                    throw new ArgumentOutOfRangeException(nameof(source), "Collection must contain at least 2 item");

                second = enumerator.Current;

                rest = YieldRest(enumerator);
            }

            public void Deconstruct(out TSource? first, out TSource? second, out TSource? third, out IEnumerable<TSource> rest)
            {
                ArgumentNullException.ThrowIfNull(source);

                using var enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    throw new ArgumentOutOfRangeException(nameof(source), "Collection must contain at least 3 item");

                first = enumerator.Current;

                if (!enumerator.MoveNext())
                    throw new ArgumentOutOfRangeException(nameof(source), "Collection must contain at least 3 item");

                second = enumerator.Current;

                if (!enumerator.MoveNext())
                    throw new ArgumentOutOfRangeException(nameof(source), "Collection must contain at least 3 item");

                third = enumerator.Current;

                rest = YieldRest(enumerator);
            }
        }

        private static IEnumerable<T> YieldRest<T>(IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
}

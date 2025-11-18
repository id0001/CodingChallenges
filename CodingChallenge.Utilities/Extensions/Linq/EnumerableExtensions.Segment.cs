namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public IEnumerable<IEnumerable<TSource>> Segment(Func<TSource, bool> newSegmentPredicate)
                => Segment(source, (curr, _, _) => newSegmentPredicate(curr), x => x);

            public IEnumerable<IEnumerable<TSource>> Segment(Func<TSource, int, bool> newSegmentPredicate)
                => Segment(source, (curr, _, index) => newSegmentPredicate(curr, index), x => x);
        }

        private static IEnumerable<TResult> Segment<TSource, TResult>(IEnumerable<TSource> source,
            Func<TSource, TSource, int, bool> newSegmentpredicate, Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(newSegmentpredicate);
            ArgumentNullException.ThrowIfNull(resultSelector);

            using var iter = source.GetEnumerator();

            if (!iter.MoveNext())
                yield break;

            var previous = iter.Current;
            List<TSource> segment = [previous];

            for (var i = 1; iter.MoveNext(); i++)
            {
                var current = iter.Current;
                if (newSegmentpredicate(current, previous, i))
                {
                    yield return resultSelector(segment);
                    segment = [current];
                }
                else
                {
                    segment.Add(current);
                }

                previous = current;
            }

            yield return resultSelector(segment);
        }
    }
}

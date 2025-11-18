namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public IEnumerable<(TSource Current, TSource Next)> CurrentAndNext(bool wrapAround = false)
            {
                ArgumentNullException.ThrowIfNull(source);

                using var e = source.GetEnumerator();
                if (!e.MoveNext())
                    yield break;

                var previous = e.Current;
                var first = previous;
                while (e.MoveNext())
                {
                    yield return (previous, e.Current);
                    previous = e.Current;
                }

                if (wrapAround) yield return (previous, first);
            }
        }
    }
}

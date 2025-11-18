namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public IEnumerable<TSource> Cycle()
            {
                ArgumentNullException.ThrowIfNull(source);

                IEnumerator<TSource>? enumerator = null;

                try
                {
                    enumerator = source.GetEnumerator();

                    while (true)
                    {
                        if (!enumerator.MoveNext())
                        {
                            enumerator.Dispose();
                            enumerator = source.GetEnumerator();
                            continue;
                        }

                        yield return enumerator.Current;
                    }
                }
                finally
                {
                    enumerator?.Dispose();
                }
            }
        }
    }
}
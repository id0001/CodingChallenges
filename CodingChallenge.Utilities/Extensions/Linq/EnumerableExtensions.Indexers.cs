namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public TSource Second()
            {
                ArgumentNullException.ThrowIfNull(source);
                return source.ElementAt(1);
            }

            public TSource Third()
            {
                ArgumentNullException.ThrowIfNull(source);
                return source.ElementAt(2);
            }

            public TSource Fourth()
            {
                ArgumentNullException.ThrowIfNull(source);
                return source.ElementAt(3);
            }

            public TSource Fifth()
            {
                ArgumentNullException.ThrowIfNull(source);
                return source.ElementAt(4);
            }
        }
    }
}

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<TSource>(Grid2<TSource> source)
        {
            public Grid2<TSource> Fill(TSource value)
            {
                ArgumentNullException.ThrowIfNull(source);

                foreach (var c in source.Keys)
                    source[c] = value;

                return source;
            }

            public Grid2<TSource> Fill(Func<Point2, TSource> selector)
            {
                ArgumentNullException.ThrowIfNull(source);

                foreach (var c in source.Keys)
                    source[c] = selector(c);

                return source;
            }
        }
    }
}

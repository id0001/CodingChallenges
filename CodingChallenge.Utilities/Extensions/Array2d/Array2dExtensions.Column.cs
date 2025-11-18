namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<TSource>(TSource[,] source)
        {
            public IEnumerable<TSource> Column(int column)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentOutOfRangeException.ThrowIfNegative(column);
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(column, source.GetLength(1));

                for (var i = 0; i < source.GetLength(0); i++)
                    yield return source[i, column];
            }

            public IEnumerable<IEnumerable<TSource>> Columns()
            {
                ArgumentNullException.ThrowIfNull(source);

                return Enumerable.Range(0, source.GetLength(1)).Select(c => Column(source, c));
            }
        }

        extension<TSource>(Grid2<TSource> source)
        {
            public IEnumerable<TSource> Column(int column)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentOutOfRangeException.ThrowIfNegative(column);
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(column, source.Bounds.Width);

                for (var i = 0; i < source.Bounds.Height; i++)
                    yield return source[i, column];
            }

            public IEnumerable<IEnumerable<TSource>> Columns()
            {
                ArgumentNullException.ThrowIfNull(source);

                return Enumerable.Range(0, source.Bounds.Width).Select(c => Column(source, c));
            }
        }
    }
}

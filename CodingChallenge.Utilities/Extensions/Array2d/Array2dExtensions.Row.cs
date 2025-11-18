namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<TSource>(TSource[,] source)
        {
            public IEnumerable<TSource> Row(int row)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentOutOfRangeException.ThrowIfNegative(row);
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(row, source.GetLength(0));

                for (var i = 0; i < source.GetLength(1); i++)
                    yield return source[row, i];
            }

            public IEnumerable<IEnumerable<TSource>> Rows()
            {
                ArgumentNullException.ThrowIfNull(source);

                return Enumerable.Range(0, source.GetLength(0)).Select(r => Row(source, r));
            }
        }
        
        extension<TSource>(Grid2<TSource> source)
        {
            public IEnumerable<TSource> Row(int row)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentOutOfRangeException.ThrowIfNegative(row);
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(row, source.Bounds.Width);

                for (var i = 0; i < source.Bounds.Width; i++)
                    yield return source[row, i];
            }

            public IEnumerable<IEnumerable<TSource>> Rows()
            {
                ArgumentNullException.ThrowIfNull(source);

                return Enumerable.Range(0, source.Bounds.Height).Select(r => Row(source, r));
            }
        }
    }
}

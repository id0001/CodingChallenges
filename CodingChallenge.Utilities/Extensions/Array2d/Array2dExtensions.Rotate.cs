namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<T>(Grid2<T> source)
        {
            public Grid2<T> Rotate90()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.ColumnCount, source.RowCount);
                for (var y = 0; y < source.RowCount; y++)
                    for (var x = 0; x < source.ColumnCount; x++)
                        updated[x, source.ColumnCount - 1 - y] = source[y, x];

                return updated;
            }

            public Grid2<T> Rotate180()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.RowCount, source.ColumnCount);
                for (var y = 0; y < source.RowCount; y++)
                    for (var x = 0; x < source.ColumnCount; x++)
                        updated[updated.RowCount - 1 - y, updated.ColumnCount - 1 - x] = source[y, x];

                return updated;
            }

            public Grid2<T> Rotate270()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.ColumnCount, source.RowCount);
                for (var y = 0; y < source.RowCount; y++)
                    for (var x = 0; x < source.ColumnCount; x++)
                        updated[source.RowCount - 1 - x, y] = source[y, x];

                return updated;
            }
        }
    }
}

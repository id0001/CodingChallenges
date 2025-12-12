namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<T>(Grid2<T> source)
        {
            public Grid2<T> Rotate90()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.Columns, source.Rows);
                for (var y = 0; y < source.Rows; y++)
                    for (var x = 0; x < source.Columns; x++)
                        updated[x, source.Columns - 1 - y] = source[y, x];

                return updated;
            }

            public Grid2<T> Rotate180()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.Rows, source.Columns);
                for (var y = 0; y < source.Rows; y++)
                    for (var x = 0; x < source.Columns; x++)
                        updated[updated.Rows - 1 - y, updated.Columns - 1 - x] = source[y, x];

                return updated;
            }

            public Grid2<T> Rotate270()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.Columns, source.Rows);
                for (var y = 0; y < source.Rows; y++)
                    for (var x = 0; x < source.Columns; x++)
                        updated[source.Rows - 1 - x, y] = source[y, x];

                return updated;
            }
        }
    }
}

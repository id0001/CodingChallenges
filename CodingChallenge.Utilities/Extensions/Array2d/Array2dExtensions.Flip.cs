namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<T>(Grid2<T> source)
        {
            public Grid2<T> FlipVertical()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.RowCount, source.ColumnCount);
                for (var y = 0; y < source.RowCount; y++)
                    for (var x = 0; x < source.ColumnCount; x++)
                        updated[updated.RowCount - 1 - y, x] = source[y, x];

                return updated;
            }

            public Grid2<T> FlipHorizontal()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.RowCount, source.ColumnCount);
                for (var y = 0; y < source.RowCount; y++)
                    for (var x = 0; x < source.ColumnCount; x++)
                        updated[y, updated.ColumnCount - 1 - x] = source[y, x];

                return updated;
            }
        }
    }
}

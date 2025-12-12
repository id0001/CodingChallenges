namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<T>(Grid2<T> source)
        {
            public Grid2<T> FlipVertical()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.Rows, source.Columns);
                for (var y = 0; y < source.Rows; y++)
                    for (var x = 0; x < source.Columns; x++)
                        updated[updated.Rows - 1 - y, x] = source[y, x];

                return updated;
            }

            public Grid2<T> FlipHorizontal()
            {
                ArgumentNullException.ThrowIfNull(source);

                var updated = new Grid2<T>(source.Rows, source.Columns);
                for (var y = 0; y < source.Rows; y++)
                    for (var x = 0; x < source.Columns; x++)
                        updated[y, updated.Columns - 1 - x] = source[y, x];

                return updated;
            }
        }
    }
}

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<TSource>(TSource[][] source)
        {
            public TSource[,] To2dArray()
            {
                ArgumentNullException.ThrowIfNull(source);

                if (source.Length == 0 || source[0].Length == 0)
                    throw new ArgumentOutOfRangeException(nameof(source), "Array cannot be empty");

                if (source.DistinctBy(col => col.Length).Count() != 1)
                    throw new ArgumentOutOfRangeException(nameof(source), "All columns must be of equal length");

                var result = new TSource[source.Length, source[0].Length];
                for (var y = 0; y < source.Length; y++)
                {
                    for (var x = 0; x < source[y].Length; x++)
                        result[y, x] = source[y][x];
                }

                return result;
            }
        }

        extension<TSource>(Grid2<TSource> source)
        {
            public TSource[,] To2dArray()
            {
                ArgumentNullException.ThrowIfNull(source);

                var result = new TSource[source.RowCount, source.ColumnCount];
                for (var y = 0; y < source.RowCount; y++)
                {
                    for (var x = 0; x < source.ColumnCount; x++)
                        result[y, x] = source[y, x];
                }

                return result;
            }
        }
    }
}

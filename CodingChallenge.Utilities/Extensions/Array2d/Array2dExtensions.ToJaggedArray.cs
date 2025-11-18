namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<TSource>(TSource[,] source)
        {
            public TSource[][] ToJaggedArray()
            {
                ArgumentNullException.ThrowIfNull(source);

                var result = new TSource[source.GetLength(0)][];
                for (var y = 0; y < source.GetLength(0); y++)
                {
                    result[y] = new TSource[source.GetLength(1)];
                    for (var x = 0; x < source.GetLength(1); x++)
                        result[y][x] = source[y, x];
                }

                return result;
            }
        }
    }
}

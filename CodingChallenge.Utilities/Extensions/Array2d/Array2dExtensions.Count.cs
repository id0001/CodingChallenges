namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<TSource>(TSource[,] source)
        {
            public int Count(Func<TSource, bool> predicate)
            {
                ArgumentNullException.ThrowIfNull(source);

                var count = 0;
                for (var y = 0; y < source.GetLength(0); y++)
                {
                    for (var x = 0; x < source.GetLength(1); x++)
                    {
                        if (predicate(source[y, x]))
                            count++;
                    }
                }

                return count;
            }
        }
    }
}

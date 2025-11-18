using System.Numerics;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension(int[,] source)
        {
            public int Sum()
            {
                ArgumentNullException.ThrowIfNull(source);

                return Sum<int, int>(source);
            }
        }

        extension<TSource>(TSource[,] source) where TSource : INumber<TSource>
        {
            public TResult Sum<TResult>() where TResult : INumber<TResult>
            {
                TResult sum = TResult.Zero;
                for (var y = 0; y < source.GetLength(0); y++)
                {
                    for (var x = 0; x < source.GetLength(1); x++)
                    {
                        checked { sum += TResult.CreateChecked(source[y, x]); }
                    }
                }

                return sum;
            }
        }
    }
}

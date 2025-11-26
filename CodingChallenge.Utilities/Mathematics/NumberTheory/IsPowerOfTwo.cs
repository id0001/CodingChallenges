using System.Numerics;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class NumberTheory
    {
        public static bool IsPowerOfTwo<TNumber>(TNumber number) where TNumber : IBinaryInteger<TNumber>
            => number > TNumber.Zero && (number & (number - TNumber.One)) == TNumber.Zero;
    }
}

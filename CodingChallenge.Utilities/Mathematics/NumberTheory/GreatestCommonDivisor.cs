using System.Numerics;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class NumberTheory
    {
        public static TNumber GreatestCommonDivisor<TNumber>(TNumber a, TNumber b)
            where TNumber : IBinaryInteger<TNumber>
        {
            while (b != TNumber.Zero)
            {
                TNumber num = a % b;
                a = b;
                b = num;
            }

            return TNumber.Abs(a);
        }
    }
}

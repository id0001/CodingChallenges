using System.Numerics;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class NumberTheory
    {
        public static TNumber NextPowerOf2<TNumber>(TNumber number) where TNumber : IBinaryInteger<TNumber>
        {
            if (number <= TNumber.One)
                return TNumber.One;

            TNumber power = TNumber.One;
            while (power < number)
            {
                TNumber nextPower = power << 1;
                if (nextPower <= power)
                    throw new OverflowException($"The next power of two for {number} cannot be represented by {typeof(TNumber)}.");

                power = nextPower;
            }

            return power;
        }
    }
}

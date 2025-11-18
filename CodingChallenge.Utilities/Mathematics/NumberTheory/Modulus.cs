using System.Numerics;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class NumberTheory
    {
        public static TNumber Modulus<TNumber>(TNumber dividend, TNumber divisor)
            where TNumber : IBinaryInteger<TNumber> 
            => (dividend % divisor + divisor) % divisor;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        ///     Find the modular inverse using the extended euclid algorithm.
        ///     Assumes a and m are coprimes, i.e., gcd(a,m) = 1
        /// </summary>
        /// <param name="a">The value</param>
        /// <param name="m">The modulo</param>
        /// <returns></returns>
        public static TNumber ModInverse<TNumber>(TNumber a, TNumber m)
            where TNumber : IBinaryInteger<TNumber>
        {
            TNumber m0 = m;
            TNumber y = TNumber.Zero;
            TNumber x = TNumber.One;

            if (m == TNumber.One)
                return TNumber.Zero;

            while(a > TNumber.One)
            {
                // q is quotient
                TNumber q = a / m;

                TNumber t = m;

                // m is remainder now, process same as Euclid's algorithm
                m = a % m;
                a = t;
                t = y;

                // Update x and y
                y = x - q * y;
                x = t;
            }

            if (x < TNumber.Zero)
                x += m0;

            return x;
        }
    }
}

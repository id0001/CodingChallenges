using System.Numerics;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class NumberTheory
    {
        /// <summary>
        ///     Find all divisors for a given integer
        /// </summary>
        /// <typeparam name="TNumber"></typeparam>
        /// <param name="number"></param>
        /// <returns></returns>
        public static IEnumerable<TNumber> Divisors<TNumber>(TNumber number) where TNumber : IBinaryInteger<TNumber>
        {
            for (var i = TNumber.One; i <= number; i++)
                if (number % i == TNumber.Zero)
                    yield return i;
        }

        /// <summary>
        ///     Calculate the sum of the divisors to the power of k
        /// </summary>
        /// <typeparam name="TNumber"></typeparam>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static TNumber DivisorSigma<TNumber>(TNumber n, TNumber k)
            where TNumber : IPowerFunctions<TNumber>, IComparisonOperators<TNumber, TNumber, bool>,
            IModulusOperators<TNumber, TNumber, TNumber>
        {
            var sum = TNumber.Zero;
            for (var i = TNumber.One; i <= n; i++)
                if (n % i == TNumber.Zero)
                    sum += TNumber.Pow(i, k);

            return sum;
        }
    }
}

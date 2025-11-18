using System.Numerics;
using CodingChallenge.Utilities.Extensions;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class NumberTheory
    {
        public static TNumber ChineseRemainderTheorem<TNumber>(TNumber[] moduli, TNumber[] remainders)
            where TNumber : IBinaryInteger<TNumber>
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(moduli.Length, remainders.Length);

            var moduliProduct = moduli.Product();
            var x = TNumber.Zero;

            for (var i = 0; i < moduli.Length; i++)
            {
                var ni = moduliProduct / moduli[i];
                var mi = ModInverse(ni, moduli[i]);
                x += remainders[i] * ni * mi;
            }

            return x.Mod(moduliProduct);
        }
    }
}

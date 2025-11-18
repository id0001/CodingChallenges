using System.Numerics;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class NumberTheory
    {
        public static TNumber TriangularNumber<TNumber>(TNumber n)
            where TNumber : IBinaryInteger<TNumber>
            => n * (n + TNumber.One) / TNumber.CreateChecked(2);

        public static TNumber InverseTriangularNumber<TNumber>(TNumber n)
            where TNumber : IBinaryInteger<TNumber> 
            => TNumber.CreateChecked(Math.Floor(Math.Sqrt(double.CreateChecked(n) * 2d)));
    }
}

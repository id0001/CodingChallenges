using CodingChallenge.Utilities.Mathematics;
using System.Globalization;
using System.Numerics;

namespace CodingChallenge.Utilities.Extensions
{
    public static class NumberExtensions
    {
        extension<TNumber>(TNumber source)
            where TNumber : IBinaryInteger<TNumber>
        {
            public TNumber Mod(TNumber divisor) => NumberTheory.Modulus(source, divisor);

            public string ToHexString(int padding = 2, bool upperCase = false)
                => source.ToString($"{(upperCase ? "X" : "x")}{padding}", CultureInfo.InvariantCulture);

            public byte[] Digits() => [.. EnumerateDigits(source, TNumber.CreateChecked(10))];

            public int DigitCount => (int)Math.Floor(Math.Log10(double.CreateChecked(source))) + 1;

            public byte DigitAt(int indexFromRight)
            {
                var value = double.CreateChecked(source);
                var divisor = Math.Pow(10d, indexFromRight);
                var divisor2 = Math.Pow(10d, indexFromRight + 1);
                return byte.CreateChecked(Math.Truncate(value / divisor) - 10d * Math.Truncate(value / divisor2));
            }

            public TNumber SetBit(int index, bool value)
            {
                var bits = ulong.CreateChecked(source);
                bits = value ? (bits | (1ul << index)) : (bits & ~(1ul << index));
                return TNumber.CreateChecked(bits);
            }
        }

        extension<TNumber>(TNumber source)
            where TNumber : IBinaryInteger<TNumber>, IBitwiseOperators<TNumber, int, TNumber>
        {
            public bool IsBitSet(int index) => ((source >> index) & TNumber.One) != TNumber.Zero;
        }

        private static IEnumerable<byte> EnumerateDigits<TNumber>(TNumber number, TNumber numberBase)
            where TNumber : IBinaryInteger<TNumber>
        {
            if (number < numberBase)
                yield return byte.CreateChecked(number);
            else
                foreach (var n in EnumerateDigits(number / numberBase, numberBase).Concat(EnumerateDigits(number % numberBase, numberBase)))
                    yield return n;
        }

        extension(int source)
        {
            public Point2 ToPoint2(int width) => new Point2(source % width, source / width);
        }
    }
}

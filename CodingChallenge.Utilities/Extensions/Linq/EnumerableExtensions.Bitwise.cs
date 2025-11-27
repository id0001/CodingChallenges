using System.Numerics;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TNumber>(IEnumerable<TNumber> source)
            where TNumber : IBitwiseOperators<TNumber, TNumber, TNumber>
        {
            public TNumber Xor()
            {
                ArgumentNullException.ThrowIfNull(source);

                return source.Aggregate((TNumber a, TNumber b) => a ^ b);
            }
        }
    }
}

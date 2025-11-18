using System.Numerics;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TNumber>(IEnumerable<TNumber> source)
            where TNumber : INumber<TNumber>, IMultiplyOperators<TNumber, TNumber, TNumber>
        {
            public TNumber Product()
            {
                ArgumentNullException.ThrowIfNull(source);

                return source.Aggregate((product, item) => product * item);
            }
        }
    }
}

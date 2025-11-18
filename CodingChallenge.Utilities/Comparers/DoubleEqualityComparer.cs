using System.Diagnostics.CodeAnalysis;
using CodingChallenge.Utilities.Extensions;

namespace CodingChallenge.Utilities.Comparers
{
    public class DoubleEqualityComparer(uint precision = 6) : IEqualityComparer<double>
    {
        public static readonly DoubleEqualityComparer Default = new();

        public bool Equals(double x, double y) => x.Equals(y, precision);

        public int GetHashCode([DisallowNull] double obj) => obj.GetHashCode();
    }
}

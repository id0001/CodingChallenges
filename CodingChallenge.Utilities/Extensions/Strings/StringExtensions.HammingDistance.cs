namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public int HammingDistance(string other)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(other);
                if (source.Length != other.Length)
                    throw new ArgumentException("Parameter must be of equal length.", nameof(other));

                return source.Zip(other).Count(pair => pair.First != pair.Second);
            }
        }
    }
}

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public bool IsAnagramOf(string other)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(other);

                if (source == other)
                    return true;

                if (source.Length != other.Length)
                    return false;

                return source.Order().SequenceEqual(other.Order());
            }
        }
    }
}

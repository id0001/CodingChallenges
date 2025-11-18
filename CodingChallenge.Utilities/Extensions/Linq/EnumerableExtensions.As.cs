namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension(IEnumerable<IConvertible> source)
        {
            public IEnumerable<T> As<T>() where T : IConvertible
            {
                ArgumentNullException.ThrowIfNull(source);

                return source.Select(x => x.As<T>());
            }
        }

        extension(IEnumerable<char> source)
        {
            public string AsString()
            {
                ArgumentNullException.ThrowIfNull(source);

                return string.Concat(source);
            }
        }

        extension(char[] source)
        {
            public string AsString()
            {
                ArgumentNullException.ThrowIfNull(source);

                return new string(source);
            }
        }
    }
}

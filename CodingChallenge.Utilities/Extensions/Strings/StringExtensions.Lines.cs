namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public IEnumerable<string> Lines()
            {
                ArgumentNullException.ThrowIfNull(source);

                return source.SplitBy(Environment.NewLine);
            }

            public IEnumerable<T> Lines<T>(Func<string, T> selector)
            {
                ArgumentNullException.ThrowIfNull(source);

                return source.SplitBy(Environment.NewLine).Select(selector);
            }
        }
    }
}

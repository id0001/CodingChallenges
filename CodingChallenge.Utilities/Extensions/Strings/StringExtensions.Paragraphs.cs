namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public IEnumerable<string> Paragraphs()
            {
                ArgumentNullException.ThrowIfNull(source);

                return source.SplitBy($"{Environment.NewLine!}{Environment.NewLine}");
            }
        }
    }
}

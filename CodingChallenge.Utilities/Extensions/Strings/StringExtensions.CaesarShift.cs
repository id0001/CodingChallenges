namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public string CaesarShift(int shift)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);

                return string.Join(string.Empty, source.ToLowerInvariant().Select(c => (char)('a' + (c - 'a' + shift) % 26)));
            }
        }
    }
}

using System.Collections.Immutable;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension(IEnumerable<char> source)
        {
            public ImmutableDictionary<char, int> GetCharCount()
            {
                ArgumentNullException.ThrowIfNull(source);

                var dict = "abcdefghijklmnopqrstuvwxyz".ToImmutableDictionary(kv => kv, kv => 0);
                return source.Aggregate(dict, (dict, c) => dict.SetItem(c, dict[c] + 1));
            }
        }
    }
}

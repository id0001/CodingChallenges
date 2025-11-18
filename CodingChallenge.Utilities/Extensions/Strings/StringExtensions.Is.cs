using System.Globalization;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public bool Is<T>(out T value) where T : IParsable<T> => T.TryParse(source, CultureInfo.InvariantCulture, out value!);

            public bool Is<T>() where T : IParsable<T> => T.TryParse(source, CultureInfo.InvariantCulture, out _);
        }
    }
}

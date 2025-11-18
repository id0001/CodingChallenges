using System.Text;

namespace CodingChallenge.Utilities.Extensions
{
    public static class StringBuilderExtensions
    {
        extension(StringBuilder source)
        {
            public StringBuilder Truncate(int length)
            {
                ArgumentNullException.ThrowIfNull(source);

                source.Length = length;
                return source;
            }

            public StringBuilder Shift(int shiftAmount)
            {
                ArgumentNullException.ThrowIfNull(source);

                shiftAmount = shiftAmount.Mod(source.Length);

                if (shiftAmount == 0)
                    return source;

                if (shiftAmount > 0)
                {
                    string s = source.ToString(source.Length - shiftAmount, shiftAmount);
                    return source.Truncate(source.Length - shiftAmount).Insert(0, s);
                }
                else
                {
                    string s = source.ToString(0, shiftAmount);
                    return source.Remove(0, shiftAmount).Append(s);
                }
            }

            public int IndexOf(char c)
            {
                ArgumentNullException.ThrowIfNull(source);

                for (var i = 0; i < source.Length; i++)
                {
                    if (source[i] == c)
                        return i;
                }

                return -1;
            }
        }
    }
}

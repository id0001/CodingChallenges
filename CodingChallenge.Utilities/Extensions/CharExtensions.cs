namespace CodingChallenge.Utilities.Extensions
{
    public static class CharExtensions
    {
        extension(char source)
        {
            public int AsInteger() => (int)char.GetNumericValue(source);

            public double AsDecimal() => char.GetNumericValue(source);
        }
    }
}

namespace CodingChallenge.Utilities.Extensions
{
    public static class DoubleExtensions
    {
        extension(double source)
        {
            public bool Equals(double other, uint precision) => Math.Abs(other - source) < Math.Pow(10, -precision);
        }
    }
}

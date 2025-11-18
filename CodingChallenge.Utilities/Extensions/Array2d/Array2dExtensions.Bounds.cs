namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<TSource>(TSource[,] source)
        {
            public Rectangle Bounds
            {
                get
                {
                    ArgumentNullException.ThrowIfNull(source);
                    return new Rectangle(0, 0, source.GetLength(1), source.GetLength(0));
                }
            }
        }
    }
}

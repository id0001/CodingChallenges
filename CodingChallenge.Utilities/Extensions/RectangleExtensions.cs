namespace CodingChallenge.Utilities.Extensions
{
    public static class RectangleExtensions
    {
        extension(Rectangle source)
        {
            public IEnumerable<Point2> EnumeratePointsInRectangle()
            {
                for (var y = source.Top; y < source.Bottom; y++)
                    for (var x = source.Left; x < source.Right; x++)
                        yield return new Point2(x, y);
            }

            public IEnumerable<Point2> EnumerateBorder()
            {
                for (var x = source.Left; x < source.Right; x++)
                    yield return new Point2(x, source.Top);

                for (var y = source.Top; y < source.Bottom; y++)
                    yield return new Point2(source.Right - 1, y);

                for (var x = source.Right - 1; x >= source.Left; x--)
                    yield return new Point2(x, source.Bottom - 1);

                for (var y = source.Bottom - 1; y >= source.Top; y--)
                    yield return new Point2(source.Left, y);
            }
        }
    }
}

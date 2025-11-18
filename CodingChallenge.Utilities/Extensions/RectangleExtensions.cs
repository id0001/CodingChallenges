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
        }
    }
}

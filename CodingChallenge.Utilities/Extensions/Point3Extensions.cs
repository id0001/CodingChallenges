namespace CodingChallenge.Utilities.Extensions
{
    public static class Point3Extensions
    {
        extension(Point3 source)
        {
            public Point2 ToPoint2() => new Point2(source.X, source.Y);
        }
    }
}

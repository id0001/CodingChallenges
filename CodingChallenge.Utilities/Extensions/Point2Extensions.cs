namespace CodingChallenge.Utilities.Extensions
{
    public static class Point2Extensions
    {
        extension(Point2 source)
        {
            public Vector2 ToVector2() => new Vector2(source.X, source.Y);
        }
    }
}

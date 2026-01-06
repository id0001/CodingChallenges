namespace CodingChallenge.Utilities.Extensions
{
    public static class Point2Extensions
    {
        extension(Point2)
        {
            public static int ToIndex(int x, int y, int width) => y * width + x;

            public static Point2 GridFlip(FlipDirection direction, int x, int y, int rows, int columns) => (direction, new Point2(x,y)) switch
            {
                (FlipDirection.Vertical, var p) => new Point2(columns - p.X - 1, p.Y),
                (FlipDirection.Horizontal, var p) => new Point2(p.X, rows - p.Y - 1),
                _ => throw new NotImplementedException(),
            };

            public static Point2 GridRotate(Rotation rotation, int x, int y, int rows, int columns) => (rotation, new Point2(x,y)) switch
            {
                (Rotation.Degree90, var p) => new Point2(rows-p.Y-1, p.X),
                (Rotation.Degree180, var p) => new Point2(columns - p.X - 1, rows - p.Y - 1),
                (Rotation.Degree270, var p) => new Point2(p.Y, columns-p.X-1),
                _ => throw new NotImplementedException(),
            };
        }

        extension(Point2 source)
        {
            public Vector2 ToVector2() => new Vector2(source.X, source.Y);

            public int ToIndex(int width) => Point2.ToIndex(source.X, source.Y, width);

            public Point2 GridFlip(FlipDirection direction, int rows, int columns) => Point2.GridFlip(direction, source.X, source.Y, rows, columns);

            public Point2 GridRotate(Rotation rotation, int rows, int columns) => Point2.GridRotate(rotation, source.X, source.Y, rows, columns);
        }
    }
}

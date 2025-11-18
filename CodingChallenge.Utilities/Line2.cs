namespace CodingChallenge.Utilities
{
    public abstract record Line2
    {
        public static Line2 FromCoords(Vector2 a, Vector2 b)
        {
            if (a.X == b.X)
                return new VerticalLine2(a.X);

            if (a.Y == b.Y)
                return new HorizontalLine2(a.Y);

            var (x1, x2, y1, y2) = a.X <= b.X ? (a.X, b.X, a.Y, b.Y) : (b.X, a.X, b.Y, a.Y);
            var slope = (y2 - y1) / (x2 - x1);
            var offset = y1 - (x1 * slope);

            return new DiagonalLine2(slope, offset);
        }

        public bool IsParallelWith(Line2 other)
        {
            if (this is HorizontalLine2 && other is HorizontalLine2)
                return true;

            if (this is VerticalLine2 && other is VerticalLine2)
                return true;

            if (this is DiagonalLine2 d1 && other is DiagonalLine2 d2)
                return d1.Slope != d2.Slope;

            return false;
        }

        public Vector2? Intersects(Line2 other)
        {
            if (IsParallelWith(other))
                return null;

            if (this is HorizontalLine2 h1)
            {
                return other switch
                {
                    VerticalLine2 v2 => new Vector2(v2.X, h1.Y),
                    DiagonalLine2 d2 => new Vector2((h1.Y - d2.Offset) / d2.Slope, h1.Y),
                    _ => throw new NotImplementedException()
                };
            }

            if (this is VerticalLine2 v1)
            {
                return other switch
                {
                    HorizontalLine2 h2 => new Vector2(v1.X, h2.Y),
                    DiagonalLine2 d2 => new Vector2(v1.X, (d2.Slope * v1.X) + d2.Offset),
                    _ => throw new NotImplementedException()
                };
            }

            if (this is DiagonalLine2 d1)
            {
                return other switch
                {
                    HorizontalLine2 h2 => new Vector2((h2.Y - d1.Offset) / d1.Slope, h2.Y),
                    VerticalLine2 v2 => new Vector2(v2.X, (d1.Slope * v2.X) + d1.Offset),
                    DiagonalLine2 d2 => new Vector2((d2.Offset - d1.Offset) / (d1.Slope - d2.Slope), (d1.Slope * (d2.Offset - d1.Offset) / (d1.Slope - d2.Slope)) + d1.Offset),
                    _ => throw new NotImplementedException()
                };
            }

            return null;
        }
    }

    public sealed record DiagonalLine2(double Slope, double Offset) : Line2;

    public sealed record VerticalLine2(double X) : Line2;

    public sealed record HorizontalLine2(double Y) : Line2;
}

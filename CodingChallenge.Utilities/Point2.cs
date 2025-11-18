namespace CodingChallenge.Utilities
{
    public readonly record struct Point2(int X, int Y)
    {
        public static readonly Point2 Zero = new();
        public static readonly Point2 One = new(1, 1);

        public Point2 Left => new(X - 1, Y);

        public Point2 Right => new(X + 1, Y);

        public Point2 Up => new(X, Y - 1);

        public Point2 Down => new(X, Y + 1);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString() => $"{X},{Y}".ToString();

        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

        public IEnumerable<Point2> GetNeighbors(bool includeDiagonal = false)
        {
            for (var y = -1; y <= 1; y++)
                for (var x = -1; x <= 1; x++)
                {
                    if (!includeDiagonal && !((x == 0) ^ (y == 0)))
                        continue;

                    if (x == 0 && y == 0)
                        continue;

                    yield return new Point2(X + x, Y + y);
                }
        }

        public IEnumerable<Point2> ManhattanBorder(int distance)
        {
            for (var x = X - distance; x <= X + distance; x++)
            {
                var dy = distance - Math.Abs(X - x);

                yield return new Point2(x, Y - dy);
                if (dy > 0)
                    yield return new Point2(x, Y + dy);
            }
        }

        /// <summary>
        ///     Rotates a point around a pivot on a circle by the amount defined by angle
        /// </summary>
        /// <param name="point">The point to move</param>
        /// <param name="pivot">The pivot to rotate around</param>
        /// <param name="angle">The angle in radians to rotate by</param>
        /// <returns>The new rotated point</returns>
        public Point2 TurnAround(Point2 pivot, double angle)
        {
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);
            var (dx, dy) = this - pivot;

            var x = pivot.X + (int)Math.Round(cos * dx - sin * dy);
            var y = pivot.Y + (int)Math.Round(sin * dx + cos * dy);
            return new Point2(x, y);
        }

        public static IEnumerable<Point2> BressenhamLine(Point2 from, Point2 to)
        {
            if (Math.Abs(to.Y - from.Y) < Math.Abs(to.X - from.X))
            {
                if (from.X > to.X)
                    return BresenhamLineLow(to.X, to.Y, from.X, from.Y).Reverse();
                return BresenhamLineLow(from.X, from.Y, to.X, to.Y);
            }

            if (from.Y > to.Y)
                return BresenhamLineHigh(to.X, to.Y, from.X, from.Y).Reverse();
            return BresenhamLineHigh(from.X, from.Y, to.X, to.Y);
        }

        public static int ManhattanDistance(Point2 p0, Point2 p1)
            => Math.Abs(p1.X - p0.X) + Math.Abs(p1.Y - p0.Y);

        private static IEnumerable<Point2> BresenhamLineHigh(int x0, int y0, int x1, int y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            var d = 2 * dx - dy;
            var x = x0;

            for (var y = y0; y <= y1; y++)
            {
                yield return new Point2(x, y);
                if (d > 0)
                {
                    x = x + xi;
                    d = d + 2 * (dx - dy);
                }
                else
                {
                    d = d + 2 * dx;
                }
            }
        }

        private static IEnumerable<Point2> BresenhamLineLow(int x0, int y0, int x1, int y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            var d = 2 * dy - dx;
            var y = y0;

            for (var x = x0; x <= x1; x++)
            {
                yield return new Point2(x, y);
                if (d > 0)
                {
                    y = y + yi;
                    d = d + 2 * (dy - dx);
                }
                else
                {
                    d = d + 2 * dy;
                }
            }
        }

        #region Operators

        public static Point2 operator +(Point2 left, Point2 right) => new(left.X + right.X, left.Y + right.Y);

        public static Point2 operator -(Point2 left, Point2 right) => new(left.X - right.X, left.Y - right.Y);

        public static Point2 operator *(Point2 left, int right) => new(left.X * right, left.Y * right);

        public static Point2 operator *(int left, Point2 right) => new(left * right.X, left * right.Y);

        public static implicit operator Point3(Point2 p) => new(p.X, p.Y, 0);

        #endregion
    }
}

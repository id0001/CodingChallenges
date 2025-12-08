namespace CodingChallenge.Utilities
{
    public readonly record struct Point3(int X, int Y, int Z)
    {
        public static readonly Point3 Zero = new();

        public Point3 Left => new(X - 1, Y, Z);

        public Point3 Right => new(X + 1, Y, Z);

        public Point3 Up => new(X, Y - 1, Z);

        public Point3 Down => new(X, Y + 1, Z);

        public Point3 Backward => new(X, Y, Z - 1);

        public Point3 Forward => new(X, Y, Z + 1);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => $"{X},{Y},{Z}".ToString();

        public void Deconstruct(out int x, out int y, out int z) => (x, y, z) = (X, Y, Z);

        public IEnumerable<Point3> GetNeighbors(bool includeDiagonal)
        {
            for (var z = -1; z <= 1; z++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    for (var x = -1; x <= 1; x++)
                    {
                        if (!includeDiagonal && !((x == 0 && y == 0) ^ (x == 0 && z == 0) ^ (y == 0 && z == 0)))
                            continue;

                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        yield return new Point3(X + x, Y + y, Z + z);
                    }
                }
            }
        }

        public static int ManhattanDistance(Point3 a, Point3 b)
            => Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y) + Math.Abs(b.Z - a.Z);

        public static long DistanceSquared(Point3 a, Point3 b)
        {
            long dx = b.X - a.X;
            long dy = b.Y - a.Y;
            long dz = b.Z - a.Z;

            return dx * dx + dy * dy + dz * dz;
        }

        #region Operators

        public static Point3 operator +(Point3 left, Point3 right) =>
            new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        public static Point3 operator -(Point3 left, Point3 right) =>
            new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

        public static Point3 operator *(Point3 left, int right) => new(left.X * right, left.Y * right, left.Z * right);

        public static Point3 operator *(int left, Point3 right) => new(left * right.X, left * right.Y, left * right.Z);

        #endregion
    }
}

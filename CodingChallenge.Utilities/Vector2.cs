namespace CodingChallenge.Utilities
{
    public readonly record struct Vector2(double X, double Y)
    {
        public static readonly Vector2 Zero = new();

        public double LengthSquared => (X * X) + (Y * Y);

        public double Length => Math.Sqrt(LengthSquared);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString() => $"{X},{Y}".ToString();

        public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

        /// <summary>
        ///     Rotates a vector around a pivot on a circle by the amount defined by angle
        /// </summary>
        /// <param name="pivot">The pivot to rotate around</param>
        /// <param name="angle">The angle in radians to rotate by</param>
        /// <returns>The new rotated vector</returns>
        public Vector2 TurnAround(Vector2 pivot, double angle)
        {
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);
            var (dx, dy) = this - pivot;

            var x = pivot.X + (int)Math.Round(cos * dx - sin * dy);
            var y = pivot.Y + (int)Math.Round(sin * dx + cos * dy);
            return new Vector2(x, y);
        }

        public static double Angle(Vector2 a, Vector2 b) => Math.Acos(Dot(a, b) / (a.Length * b.Length));

        public static double AngleOnCircle(Vector2 pivot, Vector2 a, double shift = 0)
        {
            var angle = Math.Atan2(a.Y - pivot.Y, a.X - pivot.X) + shift;
            while (angle < 0)
                angle += Math.Tau;

            while (angle > Math.Tau)
                angle -= Math.Tau;

            return angle;
        }

        public static double Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;

        public static double DistanceSquared(Vector2 a, Vector2 b) => (b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y);

        public static double Distance(Vector2 a, Vector2 b) => Math.Sqrt(DistanceSquared(a, b));

        #region Interface implementations

        #endregion

        #region Operators

        public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

        public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

        public static Vector2 operator *(Vector2 left, float right) => new(left.X * right, left.Y * right);

        public static Vector2 operator *(float left, Vector2 right) => new(left * right.X, left * right.Y);

        #endregion
    }
}

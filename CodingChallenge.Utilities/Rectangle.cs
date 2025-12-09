namespace CodingChallenge.Utilities
{
    public readonly record struct Rectangle(int X, int Y, int Width, int Height)
    {
        public static readonly Rectangle Empty = new();

        public Rectangle(Point2 position, Point2 size) : this(position.X, position.Y, size.X, size.Y)
        {
        }

        public int Left => X;

        public int Top => Y;

        public int Right => X + Width; // Exclusive

        public int Bottom => Y + Height; // Exclusive

        public int Area => Math.Abs(Width * Height);

        public long LongArea => Math.Abs((long)Width * Height);

        public override string ToString() => $"[X: {X}, Y: {Y}, Width: {Width}, Height: {Height}]";

        public bool Contains(Point2 p)
            => p.X >= Left && p.X < Right && p.Y >= Top && p.Y < Bottom;

        public bool IntersectsWith(Rectangle target) => Intersects(this, target);

        public Rectangle Intersect(Rectangle target) => Intersect(this, target);

        public Rectangle Expand(int amount)
            => new(X - amount, Y - amount, Width + amount, Height + amount);

        public static bool Intersects(Rectangle a, Rectangle b) =>
            b.Left < a.Right && a.Left < b.Right && b.Top < a.Bottom && a.Top < b.Bottom;

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            if (!a.IntersectsWith(b))
                return Empty;

            var rightSide = Math.Min(a.Right, b.Right);
            var leftSide = Math.Max(a.Left, b.Left);
            var bottomSide = Math.Min(a.Bottom, b.Bottom);
            var topSide = Math.Max(a.Top, b.Top);

            return new Rectangle(leftSide, topSide, rightSide - leftSide, bottomSide - topSide);
        }
    }
}

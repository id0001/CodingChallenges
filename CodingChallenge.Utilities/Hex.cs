namespace CodingChallenge.Utilities
{
    public readonly record struct Hex(int X, int Y, int Z)
    {
        public static readonly Hex Zero = new();

        public Hex North => new(X, Y - 1, Z + 1);

        public Hex NorthEast => new(X + 1, Y - 1, Z);

        public Hex SouthEast => new(X + 1, Y, Z - 1);

        public Hex South => new(X, Y + 1, Z - 1);

        public Hex SouthWest => new Hex(X - 1, Y + 1, Z);

        public Hex NorthWest => new Hex(X - 1, Y, Z + 1);

        public void Deconstruct(out int x, out int y, out int z) => (x, y, z) = (X, Y, Z);

        public override string ToString() => $"{X},{Y},{Z}";

        public static int ManhattanDistance(Hex a, Hex b) => (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) / 2;

        public IEnumerable<Hex> GetNeighbors()
        {
            yield return North;
            yield return NorthEast;
            yield return SouthEast;
            yield return South;
            yield return SouthWest;
            yield return NorthWest;
        }
    }
}

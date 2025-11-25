namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public Grid2<char> ToGrid()
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);

                var lines = source.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                char[,] grid = new char[lines.Length, lines[0].Length];
                for (var row = 0; row < grid.GetLength(0); row++)
                {
                    if (lines[row].Length != grid.GetLength(1))
                        throw new ArgumentException("Line lengths must be equal", nameof(source));

                    for (var col = 0; col < grid.GetLength(1); col++)
                        grid[row, col] = lines[row][col];
                }

                return new Grid2<char>(grid);
            }

            public Grid2<T> ToGrid<T>(Func<Point2, char, T> selector)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);

                var lines = source.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                T[,] grid = new T[lines.Length, lines[0].Length];
                for (var row = 0; row < grid.GetLength(0); row++)
                {
                    if (lines[row].Length != grid.GetLength(1))
                        throw new ArgumentException("Line lengths must be equal", nameof(source));

                    for (var col = 0; col < grid.GetLength(1); col++)
                        grid[row, col] = selector(new Point2(col,row), lines[row][col]);
                }

                return new Grid2<T>(grid);
            }
        }
    }
}
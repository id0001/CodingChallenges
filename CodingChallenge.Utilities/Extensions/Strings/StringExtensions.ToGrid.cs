namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public Grid2<char> ToGrid()
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);

                var lines = source.Lines().ToList();
                var maxWidth = lines.Max(l => l.Length);

                var grid = new Grid2<char>(lines.Count, maxWidth);
                for (var row = 0; row < grid.RowCount; row++)
                {
                    for (var col = 0; col < grid.ColumnCount; col++)
                        grid[row, col] = lines[row].Length > col ? lines[row][col] : ' ';
                }

                return grid;
            }

            public Grid2<T> ToGrid<T>(Func<Point2, char, T> selector)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);

                var lines = source.Lines().ToList();
                var maxWidth = lines.Max(l => l.Length);

                var grid = new Grid2<T>(lines.Count, maxWidth);
                for (var row = 0; row < grid.RowCount; row++)
                {
                    for (var col = 0; col < grid.ColumnCount; col++)
                    {
                        var p = new Point2(col, row);
                        grid[row, col] = selector(p, lines[row].Length > col ? lines[row][col] : ' ');
                    }
                }

                return grid;
            }
        }
    }
}
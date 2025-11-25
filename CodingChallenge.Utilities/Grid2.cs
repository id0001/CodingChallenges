using System.Collections;
using System.Text;

namespace CodingChallenge.Utilities
{
    public sealed class Grid2
    {
        public static Grid2<T> Fill<T>(int rows, int columns, T value) => Fill(rows, columns, _ => value);

        public static Grid2<T> Fill<T>(int rows, int columns, Func<Point2, T> fn)
        {
            var grid = new Grid2<T>(rows, columns);
            foreach (var c in grid.Keys)
                grid[c] = fn(c);

            return grid;
        }

        public static Grid2<T> From<T>(Dictionary<Point2, T> source, Func<Point2, T> fn)
        {
            int xmin = 0;
            int xmax = 0;
            int ymin = 0;
            int ymax = 0;

            foreach (var p in source.Keys)
            {
                if (p.X < xmin)
                    xmin = p.X;

                if (p.X > xmax)
                    xmax = p.X;

                if (p.Y < ymin)
                    ymin = p.Y;

                if (p.Y > ymax)
                    ymax = p.Y;
            }

            int cols = (xmax - xmin) + 1;
            int rows = (ymax - ymin) + 1;

            var pmin = new Point2(xmin, ymin);
            return Fill(rows, cols, p => fn(p + pmin));
        }
    }

    public sealed class Grid2<T> : IEnumerable<KeyValuePair<Point2, T>>
    {
        private readonly T[,] _grid;

        public Grid2(T[,] grid)
        {
            _grid = grid;
            Bounds = new Rectangle(0, 0, _grid.GetLength(1), _grid.GetLength(0));
        }

        public Grid2(int rows, int columns) : this(new T[rows, columns]) { }

        public int Columns => Bounds.Width;

        public int Rows => Bounds.Height;

        public Rectangle Bounds { get; }

        public IEnumerable<Point2> Keys => GetKeyEnumerable();

        public IEnumerable<T> Values => GetValueEnumerable();

        public T this[int row, int col]
        {
            get => _grid[row, col];
            set => _grid[row, col] = value;
        }

        public T this[Point2 coords]
        {
            get => _grid[coords.Y, coords.X];
            set => _grid[coords.Y, coords.X] = value;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var row = 0; row < _grid.GetLength(0); row++)
            {
                for (var col = 0; col < _grid.GetLength(1); col++)
                {
                    sb.Append(_grid[row, col]);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string ToString(Func<Point2, T, char> resultSelector)
        {
            var sb = new StringBuilder();
            for (var row = 0; row < _grid.GetLength(0); row++)
            {
                for (var col = 0; col < _grid.GetLength(1); col++)
                {
                    sb.Append(resultSelector(new Point2(col, row), _grid[row, col]));
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public IEnumerator<KeyValuePair<Point2, T>> GetEnumerator()
        {
            for (var y = 0; y < _grid.GetLength(0); y++)
            {
                for (var x = 0; x < _grid.GetLength(1); x++)
                {
                    yield return new KeyValuePair<Point2, T>(new Point2(x, y), _grid[y, x]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> GetValueEnumerable()
        {
            for (var y = 0; y < _grid.GetLength(0); y++)
            {
                for (var x = 0; x < _grid.GetLength(1); x++)
                {
                    yield return _grid[y, x];
                }
            }
        }

        private IEnumerable<Point2> GetKeyEnumerable()
        {
            for (var y = 0; y < _grid.GetLength(0); y++)
            {
                for (var x = 0; x < _grid.GetLength(1); x++)
                {
                    yield return new Point2(x, y);
                }
            }
        }
    }
}

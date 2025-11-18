using System.Collections;

namespace CodingChallenge.Utilities.Collections
{
    public class SpatialMap2<T> : IEnumerable<KeyValuePair<Point2, T>>
    {
        public readonly Dictionary<Point2, T> _data = [];

        public SpatialMap2()
        {
        }

        public Rectangle Bounds { get; private set; }

        public int Count => _data.Count;

        public T this[Point2 coord]
        {
            get => GetValue(coord);
            set => Set(coord, value);
        }

        public void Set(Point2 point, T value)
        {
            bool inflate = !_data.ContainsKey(point);

            _data[point] = value;

            if (inflate)
                InflateBounds(point);
        }

        public void Unset(Point2 point)
        {
            if (_data.Remove(point))
                DeflateBounds(point);
        }

        public T GetValue(Point2 coord) => _data[coord];

        public T? GetValueOrDefault(Point2 coord) => _data.GetValueOrDefault(coord);

        public T GetValueOrDefault(Point2 coord, T defaultValue) => _data.GetValueOrDefault(coord, defaultValue);

        public bool ContainsPoint(Point2 coord) => _data.ContainsKey(coord);

        private void InflateBounds(Point2 added)
        {
            int minX = Math.Min(Bounds.Left, added.X);
            int minY = Math.Min(Bounds.Top, added.Y);
            int maxX = Math.Max(Bounds.Right, added.X + 1);
            int maxY = Math.Max(Bounds.Bottom, added.Y + 1);

            Bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        private void DeflateBounds(Point2 removed)
        {
            if (removed.X != Bounds.X && removed.X + 1 != Bounds.Right && removed.Y != Bounds.Y && removed.Y + 1 != Bounds.Bottom)
                return;

            int minX = 0, minY = 0, maxX = 0, maxY = 0;
            foreach (var p in _data.Keys)
            {
                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                maxX = Math.Max(maxX, p.X + 1);
                maxY = Math.Max(maxY, p.Y + 1);
            }

            Bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public IEnumerator<KeyValuePair<Point2, T>> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();
    }
}

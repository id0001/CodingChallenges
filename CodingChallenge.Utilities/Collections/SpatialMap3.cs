using System.Collections;

namespace CodingChallenge.Utilities.Collections
{
    public class SpatialMap3<T> : IEnumerable<KeyValuePair<Point3, T>>
    {
        private readonly Dictionary<Point3, T> _data = [];
        private readonly T? _defaultValue;

        public SpatialMap3(T? defaultValue = default)
        {
            _defaultValue = defaultValue;
        }

        public Cube Bounds { get; private set; }

        public int Count => _data.Count;

        public T this[Point3 coord]
        {
            get => GetValue(coord);
            set => Set(coord, value);
        }

        public T this[int z, int y, int x]
        {
            get => GetValue(new Point3(x, y, z));
            set => Set(new Point3(x, y, z), value);
        }

        public void Set(Point3 point, T value)
        {
            bool inflate = !_data.ContainsKey(point);

            if (value is null || EqualityComparer<T>.Default.Equals(value, _defaultValue))
            {
                Unset(point);
                return;
            }

            _data[point] = value!;

            if (inflate)
                InflateBounds(point);
        }

        public void Unset(Point3 point)
        {
            if (_data.Remove(point))
                DeflateBounds(point);
        }

        public T GetValue(Point3 coord) => _data.GetValueOrDefault(coord, _defaultValue!);

        private void InflateBounds(Point3 added)
        {
            int minX = Math.Min(Bounds.Left, added.X);
            int minY = Math.Min(Bounds.Top, added.Y);
            int minZ = Math.Min(Bounds.Front, added.Z);
            int maxX = Math.Max(Bounds.Right, added.X + 1);
            int maxY = Math.Max(Bounds.Bottom, added.Y + 1);
            int maxZ = Math.Max(Bounds.Back, added.Z + 1);

            Bounds = new Cube(minX, minY, minZ, maxX - minX, maxY - minY, maxZ - minZ);
        }

        private void DeflateBounds(Point3 removed)
        {
            if (removed.X != Bounds.X && removed.X + 1 != Bounds.Right && removed.Y != Bounds.Y && removed.Y + 1 != Bounds.Bottom)
                return;

            int minX = 0, minY = 0, minZ = 0, maxX = 0, maxY = 0, maxZ = 0;
            foreach (var p in _data.Keys)
            {
                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                minZ = Math.Min(minZ, p.Z);
                maxX = Math.Max(maxX, p.X + 1);
                maxY = Math.Max(maxY, p.Y + 1);
                maxZ = Math.Max(maxZ, p.Z + 1);
            }

            Bounds = new Cube(minX, minY, minZ, maxX - minX, maxY - minY, maxZ - minZ);
        }

        public IEnumerator<KeyValuePair<Point3, T>> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();
    }
}

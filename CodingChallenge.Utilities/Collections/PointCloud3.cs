using System.Collections;

namespace CodingChallenge.Utilities.Collections
{
    public class PointCloud3 : IEnumerable<Point3>
    {
        private readonly HashSet<Point3> _points = [];

        public PointCloud3()
        {
        }

        public PointCloud3(IEnumerable<Point3> points)
        {
            foreach (var p in points)
                Set(p);
        }

        public Cube Bounds { get; private set; } = new Cube();

        public void Set(Point3 p)
        {
            if (_points.Add(p))
                InflateBounds(p);
        }

        public void Unset(Point3 p)
        {
            if (_points.Remove(p))
                DeflateBounds(p);
        }

        public bool Contains(Point3 p) => _points.Contains(p);

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
            foreach (var p in _points)
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

        public IEnumerator<Point3> GetEnumerator() => _points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();
    }
}
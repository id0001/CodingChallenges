using System.Collections;

namespace CodingChallenge.Utilities.Collections
{
    public class PointCloud2 : IEnumerable<Point2>
    {
        private readonly HashSet<Point2> _points = [];

        public PointCloud2()
        {
        }

        public PointCloud2(IEnumerable<Point2> points)
        {
            foreach (var p in points)
                Set(p);
        }

        public Rectangle Bounds { get; private set; } = new Rectangle();

        public void Set(Point2 p)
        {
            if (_points.Add(p))
                InflateBounds(p);
        }

        public void Unset(Point2 p)
        {
            if (_points.Remove(p))
                DeflateBounds(p);
        }

        public bool Contains(Point2 p) => _points.Contains(p);

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
            foreach (var p in _points)
            {
                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                maxX = Math.Max(maxX, p.X + 1);
                maxY = Math.Max(maxY, p.Y + 1);
            }

            Bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public IEnumerator<Point2> GetEnumerator() => _points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();
    }
}
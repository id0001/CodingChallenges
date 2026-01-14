using CodingChallenge.Utilities.Collections;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public SpatialMap2<char> ToSpatialMap(char defaultValue = ' ')
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);

                var y = 0;
                var map = new SpatialMap2<char>(defaultValue);
                foreach (var line in source.Lines())
                {
                    for (var x = 0; x < line.Length; x++)
                        map[y, x] = line[x];

                    y++;
                }

                return map;
            }

            public SpatialMap2<T> ToSpatialMap<T>(Func<Point2, char, T> selector, T? defaultValue = default)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);

                var y = 0;
                var map = new SpatialMap2<T>(defaultValue);
                foreach (var line in source.Lines())
                {
                    for (var x = 0; x < line.Length; x++)
                    {
                        var p = new Point2(x, y);
                        map[p] = selector(p, line[x]);
                        y++;
                    }
                }

                return map;
            }
        }
    }
}
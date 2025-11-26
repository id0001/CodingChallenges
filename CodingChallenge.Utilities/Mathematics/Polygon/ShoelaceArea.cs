using System.Numerics;
using CodingChallenge.Utilities.Extensions;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class Polygon
    {
        /// <summary>
        ///     https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static double ShoelaceArea(IEnumerable<Point2> vertices) => ShoelaceArea(vertices.Select(p => (p.X, p.Y)));

        /// <summary>
        ///     https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static double ShoelaceArea<TNumber>(IEnumerable<(TNumber X, TNumber Y)> vertices)
            where TNumber : IBinaryInteger<TNumber>
        {
            var sum1 = TNumber.Zero;
            var sum2 = TNumber.Zero;

            var first = ((TNumber X, TNumber Y)?)null;
            var last = ((TNumber X, TNumber Y)?)null;

            var vertexList = vertices.ToList();
            if (vertexList.Count == 0)
                return 0d;

            foreach (var window in vertexList.Windowed(2))
            {
                first ??= window[0];

                sum1 += window[0].X * window[1].Y;
                sum2 += window[0].Y * window[1].X;

                last = window[1];
            }

            if (first is null || last is null)
                return 0d;

            sum1 += last.Value.X * first.Value.Y;
            sum2 += last.Value.Y * first.Value.X;

            return double.CreateChecked(TNumber.Abs(sum1 - sum2)) / 2d;
        }
    }
}

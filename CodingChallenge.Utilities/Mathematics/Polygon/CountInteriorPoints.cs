using System.Numerics;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class Polygon
    {
        /// <summary>
        ///     Determine the integer points contained by the simple polygon.
        ///     Using a modified Picks formula we can derive the interior points from the area and the amount of points from the
        ///     border.
        ///     https://en.wikipedia.org/wiki/Pick's_theorem
        /// </summary>
        /// <param name="area"></param>
        /// <param name="pointsOnBoundary"></param>
        /// <returns></returns>
        public static TNumber CountInteriorPoints<TNumber>(TNumber area, TNumber pointsOnBoundary)
            where TNumber : IBinaryInteger<TNumber>
            => area - pointsOnBoundary / TNumber.CreateChecked(2) + TNumber.One;
    }
}

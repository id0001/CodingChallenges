using System.Numerics;

namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class Polygon
    {
        /// <summary>
        ///     https://en.wikipedia.org/wiki/Pick's_theorem
        /// </summary>
        /// <param name="interiorPoints"></param>
        /// <param name="pointsOnBoundary"></param>
        /// <returns></returns>
        public static TNumber PicksArea<TNumber>(TNumber interiorPoints, TNumber pointsOnBoundary)
            where TNumber : IBinaryInteger<TNumber>
            => interiorPoints + pointsOnBoundary / TNumber.CreateChecked(2) - TNumber.One;
    }
}

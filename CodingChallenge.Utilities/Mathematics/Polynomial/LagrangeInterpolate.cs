namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class Polynomial
    {
        /// <summary>
        ///     Interpolate the value of a point given the list of points
        /// </summary>
        /// <param name="points"></param>
        /// <param name="xi"></param>
        /// <returns></returns>
        public static long LagrangeInterpolate(IList<Point2> points, int xi)
        {
            var result = 0L;
            for (var i = 0; i < points.Count; i++)
            {
                var term = (long)points[i].Y;
                for (var j = 0; j < points.Count; j++)
                    if (j != i)
                        term = term * (xi - points[j].X) / (points[i].X - points[j].X);

                result += term;
            }

            return result;
        }
    }
}

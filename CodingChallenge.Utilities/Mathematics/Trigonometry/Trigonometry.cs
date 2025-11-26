namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class Trigonometry
    {
        /// <summary>
        ///     PI / 180: To convert between degree and radian
        /// </summary>
        public const double PiOver180 = 0.0174532925199432957692369076848861271344287188854172545609719144d;

        public static double Negative90 => -(Math.PI / 2d);
        public static double Positive90 => Math.PI / 2d;

        public static double DegreeToRadian(double degree) => degree * PiOver180;

        public static double RadianToDegree(double radian) => radian / PiOver180;
    }
}

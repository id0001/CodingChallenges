namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class NumberTheory
    {
        public static long LeastCommonMultiple(long a, long b, params long[] rest)
        {
            if (a == 0L && b == 0L)
                return 0L;

            var lcm = Math.Abs(long.CreateChecked(a) / GreatestCommonDivisor(a, b) * b);

            if (rest.Length == 0)
                return lcm;

            for (int i = 0; i < rest.Length; i++)
                lcm = LeastCommonMultiple(lcm, rest[i]);

            return lcm;
        }
    }
}

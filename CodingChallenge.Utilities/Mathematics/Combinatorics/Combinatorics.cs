namespace CodingChallenge.Utilities.Mathematics
{
    public static partial class Combinatorics
    {
        public static IEnumerable<int[]> GeneratePartitions(int total, int partitions, int minPartitionsSize = 0)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(partitions, 1);
            ArgumentOutOfRangeException.ThrowIfNegative(minPartitionsSize);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minPartitionsSize, total / partitions);

            if (partitions == 1)
            {
                yield return new[] { total };
                yield break;
            }

            int[] distributions = new int[partitions];
            if (minPartitionsSize > 0)
                Array.Fill(distributions, minPartitionsSize);

            do
            {
                distributions[^1] = total - distributions.Take(partitions - 1).Sum();
                yield return (int[])distributions.Clone();
                distributions[^2]++;
                distributions[^1] = minPartitionsSize;
                for (int num = partitions - 2; num >= 1; num--)
                {
                    if (distributions.Sum() <= total)
                        continue;

                    distributions[num - 1]++;
                    distributions[num] = minPartitionsSize;
                }
            }
            while (distributions.Sum() <= total);
        }
    }
}

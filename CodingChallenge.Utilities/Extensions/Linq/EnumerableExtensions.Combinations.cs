namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public IEnumerable<TSource[]> Combinations(int k)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(k, 0);

                var sourceList = source.ToList();

                if (k > sourceList.Count)
                    yield break;

                TSource[] array = new TSource[k];
                int[] c = Enumerable.Range(0, k + 2).ToArray();
                c[k] = sourceList.Count;
                c[k + 1] = 0;
                int j = k;

                if (k == sourceList.Count)
                {
                    yield return c.Take(k).Select(i => sourceList[i]).ToArray();
                    yield break;
                }

                while (true)
                {
                    for (int l = 0; l < k; l++)
                        array[l] = sourceList[c[l]];

                    yield return (TSource[])array.Clone();
                    int num;
                    if (j > 0)
                    {
                        num = j;
                    }
                    else
                    {
                        if (c[0] + 1 < c[1])
                        {
                            c[0]++;
                            continue;
                        }

                        j = 1;
                        do
                        {
                            j++;
                            c[j - 2] = j - 2;
                            num = c[j - 1] + 1;
                        }
                        while (num == c[j]);

                        if (j > k)
                            break;
                    }

                    c[j - 1] = num;
                    j--;
                }
            }
        }
    }
}

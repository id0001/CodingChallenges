namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<T>(IEnumerable<T> source)
        {
            public IEnumerable<T> MergeBy(Func<T, T, bool> shouldMerge, Func<T, T, T> merge)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(shouldMerge);
                ArgumentNullException.ThrowIfNull(merge);

                var results = new List<T>();

                int count = 0;
                do
                {
                    count = 0;
                    results = [];
                    foreach (var item in source)
                    {
                        var merged = false;
                        for (var i = 0; i < results.Count; i++)
                        {
                            if (shouldMerge(item, results[i]))
                            {
                                results[i] = merge(item, results[i]);
                                merged = true;
                                break;
                            }
                        }

                        if (!merged)
                            results.Add(item);

                        count++;
                    }

                    source = results;
                }
                while (results.Count != count);
                return results;
            }
        }
    }
}

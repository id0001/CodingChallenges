namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public IEnumerable<IReadOnlyCollection<TSource>> ChunkBy(Func<TSource, bool> predicate, bool matchIsLastItem = false)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(predicate);

                List<TSource> list = [];
                bool skipFirst = true;
                foreach(var item in source)
                {
                    if (matchIsLastItem)
                    {
                        list.Add(item);
                        if(predicate(item))
                        {
                            yield return list;
                            list = [];
                        }

                        continue;
                    }

                    if (predicate(item))
                    {
                        if (!skipFirst)
                            yield return list;

                        list = [];
                        skipFirst = false;
                    }

                    list.Add(item);
                }
            }
        }
    }
}

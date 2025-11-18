namespace CodingChallenge.Utilities.Extensions
{
    public static class LinkedListExtensions
    {
        extension<TSource>(LinkedListNode<TSource> source)
        {
            public LinkedListNode<TSource> NextOrFirst()
            {
                ArgumentNullException.ThrowIfNull(source);

                if (source.List is null)
                    throw new ArgumentException("Node is unlinked");

                return source.Next ?? source.List.First!;
            }

            public LinkedListNode<TSource> PreviousOrLast()
            {
                ArgumentNullException.ThrowIfNull(source);

                if (source.List is null)
                    throw new ArgumentException("Node is unlinked");

                return source.Previous ?? source.List!.Last!;
            }
        }
    }
}

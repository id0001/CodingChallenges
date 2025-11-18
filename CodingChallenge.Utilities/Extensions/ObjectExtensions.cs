namespace CodingChallenge.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        extension(IConvertible source)
        {
            public TResult As<TResult>() where TResult : IConvertible
            => (TResult)Convert.ChangeType(source, typeof(TResult));
        }

        extension<TSource>(TSource source)
            where TSource : notnull
        {
            public TResult Transform<TResult>(Func<TSource, TResult> transformer) where TResult : notnull
                => transformer(source);
        }
    }
}

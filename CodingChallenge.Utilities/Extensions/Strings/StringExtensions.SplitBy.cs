namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public string[] SplitBy(params string[] separators)
            => source.Split(separators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            public IEnumerable<T> SplitBy<T>(params string[] separators)
                where T : IConvertible
                => source.SplitBy(separators).As<T>();

            public (T1 First, T2 Second) SplitBy<T1, T2>(params string[] separators)
                where T1 : IConvertible
                where T2 : IConvertible
                => source.SplitBy(separators).Transform(split => (split.First().As<T1>(), split.Second().As<T2>()));
        }
    }
}

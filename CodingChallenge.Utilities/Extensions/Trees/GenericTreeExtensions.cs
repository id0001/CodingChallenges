using CodingChallenge.Utilities.Collections.Trees;

namespace CodingChallenge.Utilities.Extensions
{
    public static class GenericTreeExtensions
    {
        extension<T>(GenericTree<T> source) where T : notnull
        {
            public IEnumerable<T> Ancestors(bool includeSelf = false)
            {
                if (includeSelf)
                    yield return source.Value;

                var current = source.Parent;

                while (current is { })
                {
                    yield return current.Value;
                    current = current.Parent;
                }
            }
        }
    }
}

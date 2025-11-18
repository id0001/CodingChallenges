using CodingChallenge.Utilities.Mathematics;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class EnumerableExtensions
    {
        extension<TSource>(IEnumerable<TSource> source)
        {
            public IEnumerable<TSource[]> Permutations()
            {
                ArgumentNullException.ThrowIfNull(source);

                var array = source.ToArray();

                yield return (TSource[])array.Clone();

                if (array.Length is 0 or 1)
                    yield break;

                var permutation = Enumerable.Range(0, array.Length).ToArray();

                if (array.Length > 20)
                    throw new OverflowException("Too many permutations");

                ulong factorial = (ulong)Combinatorics.Factorial(array.Length);

                for (var n = 1ul; n < factorial; n++)
                {
                    var j = permutation.Length - 2;
                    while (permutation[j] > permutation[j + 1])
                        j--;

                    var k = permutation.Length - 1;
                    while (permutation[j] > permutation[k])
                        k--;

                    (permutation[j], permutation[k]) = (permutation[k], permutation[j]);

                    for (int x = permutation.Length - 1, y = j + 1; x > y; x--, y++)
                        (permutation[x], permutation[y]) = (permutation[y], permutation[x]);

                    var permutatedSet = new TSource[permutation.Length];
                    for (var i = 0; i < permutation.Length; i++)
                        permutatedSet[i] = array[permutation[i]];

                    yield return permutatedSet;
                }
            }
        }
    }
}

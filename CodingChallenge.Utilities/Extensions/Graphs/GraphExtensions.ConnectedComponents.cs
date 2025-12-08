using CodingChallenge.Utilities.Collections.Graphs;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class GraphExtensions
    {
        extension<TVertex>(IExplicitGraph<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public IEnumerable<IEnumerable<TVertex>> ConnectedComponents()
            {
                ArgumentNullException.ThrowIfNull(source);

                var candidates = source.Vertices.ToHashSet();
                while (candidates.Count > 0)
                {
                    var cluster = source.Bfs().FloodFill(candidates.First()).ToList();
                    foreach (var (v, _) in cluster)
                        candidates.Remove(v);

                    yield return cluster.Select(x => x.Vertex);
                }
            }
        }
    }
}

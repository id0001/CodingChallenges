using CodingChallenge.Utilities.Collections.Graphs;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class GraphExtensions
    {
        extension<TVertex>(IExplicitGraph<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public IEnumerable<TVertex> TopologicalSort()
            {
                ArgumentNullException.ThrowIfNull(source);

                List<TVertex> result = new();
                Queue<TVertex> options = new(source.Vertices.Where(v => !source.InEdges(v).Any()));
                var inDegree = source.Vertices.ToDictionary(v => v, v => source.InEdges(v).Select(x => x.Source).ToHashSet());

                while (options.Count > 0)
                {
                    var n = options.Dequeue();
                    result.Add(n);

                    foreach (var m in source.OutEdges(n).Select(e => e.Target))
                    {
                        inDegree[m].Remove(n);
                        if (inDegree[m].Count == 0)
                            options.Enqueue(m);
                    }
                }

                if (inDegree.Any(e => e.Value.Count > 0))
                    throw new ArgumentException("Graph cannot have cycles");

                return result;
            }

            public IEnumerable<TVertex> LexicographicalSort(IComparer<TVertex>? comparer = null)
            {
                ArgumentNullException.ThrowIfNull(source);

                List<TVertex> result = new();
                PriorityQueue<TVertex, TVertex> options = new(comparer ?? Comparer<TVertex>.Default);
                options.EnqueueRange(source.Vertices.Where(v => !source.InEdges(v).Any()).Select(v => (v, v)));
                var inDegree = source.Vertices.ToDictionary(v => v, v => source.InEdges(v).Select(v => v.Source).ToHashSet());

                while (options.Count > 0)
                {
                    var n = options.Dequeue();
                    result.Add(n);

                    foreach (var m in source.OutEdges(n).Select(v => v.Target))
                    {
                        inDegree[m].Remove(n);
                        if (inDegree[m].Count == 0)
                            options.Enqueue(m, m);
                    }
                }

                if (inDegree.Any(e => e.Value.Count > 0))
                    throw new ArgumentException("Graph cannot have cycles");

                return result;
            }
        }
    }
}

using CodingChallenge.Utilities.Collections.Graphs;
using System.Collections.Immutable;

namespace CodingChallenge.Utilities.Extensions.Graphs
{
    public static partial class GraphExtensions
    {
        extension<TVertex, TEdge>(IExplicitGraph<TVertex, TEdge> source)
            where TVertex : notnull, IEquatable<TVertex>
            where TEdge : notnull, Edge<TVertex>
        {
            public List<List<TVertex>> EnumerateCliques()
            {
                var r = ImmutableHashSet<TVertex>.Empty;
                var p = source.Vertices.ToImmutableHashSet();
                var x = ImmutableHashSet<TVertex>.Empty;

                var cliques = new List<List<TVertex>>();
                BronKerbosch(source, cliques, r, p, x);
                return cliques;
            }
        }

        private static void BronKerbosch<TVertex, TEdge>(
            IExplicitGraph<TVertex, TEdge> graph,
            List<List<TVertex>> cliques,
            ImmutableHashSet<TVertex> r,
            ImmutableHashSet<TVertex> p,
            ImmutableHashSet<TVertex> x
            )
                where TVertex : notnull, IEquatable<TVertex>
                where TEdge : notnull, Edge<TVertex>
        {
            if (p.Count == 0 && x.Count == 0)
            {
                cliques.Add([.. r]);
                return;
            }

            var pivot = p.Union(x).First();
            foreach (var v in p.Except(graph.OutEdges(pivot).Select(e => e.Target)))
            {
                BronKerbosch(graph, cliques, r.Add(v), p.Intersect(graph.OutEdges(v).Select(e => e.Target)), x.Intersect(graph.OutEdges(v).Select(e => e.Target)));

                p = p.Remove(v);
                x = x.Add(v);
            }
        }
    }
}

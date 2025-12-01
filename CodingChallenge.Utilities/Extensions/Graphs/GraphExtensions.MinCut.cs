using CodingChallenge.Utilities.Collections.Graphs;

namespace CodingChallenge.Utilities.Extensions.Graphs
{
    public static partial class GraphExtensions
    {
        extension<TVertex>(IExplicitGraph<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public (List<TVertex>[] Partitions, List<(TVertex, TVertex)> CutEdges) MinCut()
            {
                var random = Random.Shared;
                var vertices = new TVertex[source.Vertices.Count];
                var viLookup = new Dictionary<TVertex, int>();
                var subsets = new Subset[source.Vertices.Count];

                var vi = 0;
                foreach (var vertex in source.Vertices)
                {
                    subsets[vi] = new Subset(vi, 0);
                    vertices[vi] = vertex;
                    viLookup.Add(vertex, vi);
                    vi++;
                }

                var edges = new List<(int, int)>();
                foreach (var vertex in source.Vertices)
                {
                    foreach (var edge in source.OutEdges(vertex))
                        edges.Add((viLookup[edge.Source], viLookup[edge.Target]));
                }

                var vertexCount = vertices.Length;
                while (vertexCount > 2)
                {
                    var i = random.Next(edges.Count);

                    var subset1 = FindSubset(subsets, edges[i].Item1);
                    var subset2 = FindSubset(subsets, edges[i].Item2);

                    if (subset1 == subset2)
                        continue;

                    vertexCount--;
                    Union(subsets, subset1, subset2);
                }

                var cutEdges = new List<(TVertex, TVertex)>();
                for (var i = 0; i < edges.Count; i++)
                {
                    var subset1 = FindSubset(subsets, edges[i].Item1);
                    var subset2 = FindSubset(subsets, edges[i].Item2);
                    if (subset1 == subset2) continue;

                    var v1 = vertices[edges[i].Item1];
                    var v2 = vertices[edges[i].Item2];
                    cutEdges.Add((v1, v2));
                }

                var partitions = vertices.GroupBy(v => FindSubset(subsets, viLookup[v])).Select(e => e.ToList()).ToArray();

                return (partitions, cutEdges);
            }
        }

        private static int FindSubset(IList<Subset> subset, int v)
        {
            if (subset[v].Parent != v)
                subset[v] = subset[v] with { Parent = FindSubset(subset, subset[v].Parent) };

            return subset[v].Parent;
        }

        private static void Union(IList<Subset> subsets, int x, int y)
        {
            var xRoot = FindSubset(subsets, x);
            var yRoot = FindSubset(subsets, y);

            if (subsets[xRoot].Rank < subsets[yRoot].Rank)
            {
                subsets[xRoot] = subsets[xRoot] with { Parent = yRoot };
            }
            else if (subsets[xRoot].Rank > subsets[yRoot].Rank)
            {
                subsets[yRoot] = subsets[yRoot] with { Parent = xRoot };
            }
            else
            {
                subsets[yRoot] = subsets[yRoot] with { Parent = xRoot };
                subsets[xRoot] = subsets[xRoot] with { Rank = subsets[xRoot].Rank + 1 };
            }
        }

        private record Subset(int Parent, int Rank);
    }
}

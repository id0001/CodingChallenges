using CodingChallenge.Utilities.Collections.Graphs;
using System.Text;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class GraphExtensions
    {
        extension<TVertex>(Digraph<TVertex> source)
            where TVertex : notnull, IEquatable<TVertex>
        {
            public string ToGraphviz()
            {
                ArgumentNullException.ThrowIfNull(source);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("digraph G {");

                foreach (var vertex in source.Vertices)
                {
                    foreach (var (_, target) in source.OutEdges(vertex))
                        sb.AppendLine($"{vertex} -> {target}");
                }

                sb.Append("}");
                return sb.ToString();
            }
        }
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Collections.Graphs;

namespace AdventOfCode2015.Challenges;

[Challenge(7)]
public class Challenge07
{
    [Part(1, "3176")]
    public string Part1(string input)
    {
        var graph = new Digraph<Vertex>();
        BuildGraph(graph, input.Lines());
        return graph.Vertices.First(v => v.Name == "a").Output.ToString();
    }

    [Part(2, "14710")]
    public string Part2(string input)
    {
        var graph = new Digraph<Vertex>();
        BuildGraph(graph, input.Lines());
        var aValue = graph.Vertices.First(v => v.Name == "a").Output;

        graph.Vertices.First(x => x.Name == "b").Operation = v => aValue;
        foreach (var v in graph.Vertices)
            v.Reset();

        return graph.Vertices.First(v => v.Name == "a").Output.ToString();
    }

    private static void BuildGraph(Digraph<Vertex> graph, IEnumerable<string> lines)
    {
        var vertices = lines.SelectMany(line => CreateVertices(graph, line)).Distinct().ToDictionary(kv => kv.Name);
        foreach (var edge in lines.SelectMany(line => CreateEdges(vertices, line)))
            graph.AddEdge(edge.Source, edge.Target);
    }

    private static IEnumerable<Vertex> CreateVertices(Digraph<Vertex> graph, string line)
    {
        var split = line.SplitBy(" ");

        if (split.Length == 3) // Input
        {
            if (int.TryParse(split.First(), out var n0))
                yield return new Vertex(split.First(), v => n0);

            yield return new Vertex(split.Third(), v => graph.InEdges(v).First().Source.Output);
            yield break;
        }

        if (split.Length == 4) // Not
        {
            if (int.TryParse(split.Second(), out var n1))
                yield return new Vertex(split.Second(), v => n1);

            yield return new Vertex(split.Fourth(), v => ~graph.InEdges(v).First().Source.Output & 0xffff);
            yield break;
        }

        if (int.TryParse(split.First(), out var n2))
            yield return new Vertex(split.First(), v => n2);

        if (int.TryParse(split.Third(), out var n3))
            yield return new Vertex(split.Third(), v => n3);

        yield return new Vertex(split.Fifth(), split.Second() switch
        {
            "AND" => v => graph.InEdges(v).Select(f => f.Source.Output).Aggregate((a, b) => a & b),
            "OR" => v => graph.InEdges(v).Select(f => f.Source.Output).Aggregate((a, b) => a | b),
            "LSHIFT" => v => graph.InEdges(v).Select(f => f.Source.Output).Aggregate((a, b) => a << b),
            "RSHIFT" => v => graph.InEdges(v).Select(f => f.Source.Output).Aggregate((a, b) => a >> b),
            _ => throw new NotImplementedException(),
        });
    }

    private static IEnumerable<(Vertex Source, Vertex Target)> CreateEdges(Dictionary<string, Vertex> vertices, string line)
    {
        string[] split = line.SplitBy(" ");

        if (split.Length == 3)
        {
            yield return (vertices[split.First()], vertices[split.Third()]);
            yield break;
        }

        if (split.Length == 4)
        {
            yield return (vertices[split.Second()], vertices[split.Fourth()]);
            yield break;
        }

        yield return (vertices[split.First()], vertices[split.Fifth()]);
        yield return (vertices[split.Third()], vertices[split.Fifth()]);
    }

    private sealed class Vertex : IEquatable<Vertex>
    {
        private int? _value;

        public Vertex(string name, Func<Vertex, int> operation)
        {
            Name = name;
            Operation = operation;
        }

        public string Name { get; init; }

        public int Output => _value ??= Operation(this);

        public Func<Vertex, int> Operation { get; set; }

        public bool Equals(Vertex? other) => other is { } && other.Name == Name;

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object? obj) => Equals(obj as Vertex);

        public void Reset() => _value = null;
    }
}

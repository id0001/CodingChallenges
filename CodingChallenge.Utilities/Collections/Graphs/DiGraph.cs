
namespace CodingChallenge.Utilities.Collections.Graphs
{
    public class DiGraph<TVertex, TEdge> : IExplicitGraph<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, List<TEdge>> _vertexInEdges = new();
        private readonly Dictionary<TVertex, List<TEdge>> _vertexOutEdges = new();

        public IReadOnlySet<TVertex> Vertices => _vertexOutEdges.Keys.ToHashSet();

        public bool AddVertex(TVertex vertex)
        {
            if (ContainsVertex(vertex))
                return false;

            _vertexOutEdges.Add(vertex, []);
            _vertexInEdges.Add(vertex, []);
            return true;
        }

        public bool RemoveVertex(TVertex vertex)
        {
            if (!ContainsVertex(vertex))
                return false;

            foreach (var edge in InEdges(vertex))
                _vertexOutEdges[edge.Source].Remove(edge);

            foreach (var edge in OutEdges(vertex))
                _vertexInEdges[edge.Target].Remove(edge);

            _vertexOutEdges.Remove(vertex);
            _vertexInEdges.Remove(vertex);
            return true;
        }

        public bool AddEdge(TEdge edge)
        {
            AddVertex(edge.Source);
            AddVertex(edge.Target);

            _vertexOutEdges[edge.Source].Add(edge);
            _vertexInEdges[edge.Target].Add(edge);
            return true;
        }

        public bool ContainsVertex(TVertex vertex) => _vertexOutEdges.ContainsKey(vertex);

        public int InDegree(TVertex vertex) => _vertexInEdges[vertex].Count;

        public IEnumerable<TEdge> InEdges(TVertex vertex) => _vertexInEdges[vertex];

        public int OutDegree(TVertex vertex) => _vertexOutEdges.Count;

        public IEnumerable<TEdge> OutEdges(TVertex vertex) => _vertexOutEdges[vertex];
    }
}

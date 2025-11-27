using CodingChallenge.Utilities.Collections.Graphs;

namespace CodingChallenge.Utilities.Collections
{
    public class UndirectedGraph<TVertex, TEdge> : IExplicitGraph<TVertex, TEdge>
        where TVertex : notnull, IEquatable<TVertex>
        where TEdge : IEdge<TVertex>, IVertexReversible<TEdge>
    {
        private Dictionary<TVertex, HashSet<TEdge>> _outEdges = new();
        private Dictionary<TVertex, HashSet<TEdge>> _inEdges = new();

        public IReadOnlySet<TVertex> Vertices => throw new NotImplementedException();

        public bool AddVertex(TVertex vertex) => _edges.TryAdd(vertex, new HashSet<TEdge>());

        public bool RemoveVertex(TVertex vertex)
        {
            if (!ContainsVertex(vertex))
                return false;

            foreach (var edge in _edges[vertex])
                _edges[edge.Target].RemoveWhere(e => e.Target.Equals(vertex));

            _edges.Remove(vertex);
            return true;
        }

        public bool AddEdge(TEdge edge)
        {
            AddVertex(edge.Source);
            AddVertex(edge.Target);

            var revEdge = edge.Reverse();
            _edges[edge.Source].Add(edge);
            _edges[revEdge.Source].Add(revEdge);
            return true;
        }

        public bool RemoveEdge(TVertex a, TVertex b)
        {
            if (!ContainsVertex(a))
                return false;

            if (!ContainsVertex(b))
                return false;

            _edges[a].RemoveWhere(e => e.Target.Equals(b));
            _edges[b].RemoveWhere(e => e.Target.Equals(a));
            return true;
        }

        public bool ContainsVertex(TVertex vertex) => _edges.ContainsKey(vertex);

        public int InDegree(TVertex target) => _edges[target].Count;

        public IEnumerable<TEdge> InEdges(TVertex target) => _edges[target];

        public int OutDegree(TVertex source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEdge> OutEdges(TVertex source)
        {
            throw new NotImplementedException();
        }
    }
}

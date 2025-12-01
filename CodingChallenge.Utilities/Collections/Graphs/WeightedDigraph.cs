namespace CodingChallenge.Utilities.Collections.Graphs
{
    public class WeightedDigraph<TVertex, TWeight> : IWeightedExplicitGraph<TVertex, TWeight>, IExplicitGraph<TVertex>, IWeightedMutableGraph<TVertex, TWeight>
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull
    {
        private readonly Dictionary<TVertex, Dictionary<TVertex, TWeight>> _inEdges = [];
        private readonly Dictionary<TVertex, Dictionary<TVertex, TWeight>> _outEdges = [];

        public IReadOnlySet<TVertex> Vertices => _outEdges.Keys.ToHashSet();

        public bool AddEdge(TVertex source, TVertex target, TWeight weight)
        {
            AddVertex(source);
            AddVertex(target);

            if (_outEdges[source].ContainsKey(target))
                return false;

            _outEdges[source].Add(target, weight);
            _inEdges[target].Add(source, weight);
            return true;
        }

        public bool AddVertex(TVertex vertex)
        {
            if (ContainsVertex(vertex))
                return false;

            _outEdges.TryAdd(vertex, []);
            _inEdges.TryAdd(vertex, []);
            return true;
        }

        public bool ContainsVertex(TVertex vertex) => _outEdges.ContainsKey(vertex);

        public int InDegrees(TVertex target) => _inEdges[target].Count;

        public IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)> InEdges(TVertex target) => _inEdges[target].Select(s => (s.Key, target, s.Value));

        public int OutDegrees(TVertex source) => _outEdges[source].Count;

        public IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)> OutEdges(TVertex source) => _outEdges[source].Select(t => (source, t.Key, t.Value));

        public bool RemoveEdge(TVertex source, TVertex target)
        {
            if (!ContainsVertex(source) || !ContainsVertex(target))
                return false;

            _outEdges[source].Remove(target);
            _inEdges[target].Remove(source);
            return true;
        }

        public bool RemoveVertex(TVertex vertex)
        {
            if (!ContainsVertex(vertex))
                return false;

            foreach (var source in _inEdges[vertex].Keys)
                _outEdges[source].Remove(vertex);

            foreach (var target in _outEdges[vertex].Keys)
                _inEdges[target].Remove(vertex);

            _inEdges.Remove(vertex);
            _outEdges.Remove(vertex);

            return true;
        }

        IEnumerable<(TVertex Source, TVertex Target)> IExplicitGraph<TVertex>.InEdges(TVertex target) => InEdges(target).Select(edge => (edge.Source, edge.Target));

        IEnumerable<(TVertex Source, TVertex Target)> IImplicitGraph<TVertex>.OutEdges(TVertex source) => OutEdges(source).Select(edge => (edge.Source, edge.Target));
    }
}

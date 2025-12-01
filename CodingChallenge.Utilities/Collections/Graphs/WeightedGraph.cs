namespace CodingChallenge.Utilities.Collections.Graphs
{
    public class WeightedGraph<TVertex, TWeight> : IWeightedExplicitGraph<TVertex, TWeight>, IExplicitGraph<TVertex>, IWeightedMutableGraph<TVertex, TWeight>
        where TVertex : notnull, IEquatable<TVertex>
        where TWeight : notnull
    {
        private readonly Dictionary<TVertex, Dictionary<TVertex, TWeight>> _adjacent = [];

        public IReadOnlySet<TVertex> Vertices => _adjacent.Keys.ToHashSet();

        public bool AddVertex(TVertex vertex) => _adjacent.TryAdd(vertex, []);

        public bool RemoveVertex(TVertex vertex)
        {
            if (!ContainsVertex(vertex))
                return false;

            foreach (var (target, _) in _adjacent[vertex])
                _adjacent[target].Remove(vertex);

            _adjacent.Remove(vertex);
            return true;
        }

        public bool AddEdge(TVertex source, TVertex target, TWeight weight)
        {
            AddVertex(source);
            AddVertex(target);

            if (_adjacent[source].ContainsKey(target))
                return false;

            _adjacent[source].Add(target, weight);
            _adjacent[target].Add(source, weight);
            return true;
        }

        public bool RemoveEdge(TVertex source, TVertex target)
        {
            if (!ContainsVertex(source) || !ContainsVertex(target))
                return false;

            _adjacent[target].Remove(source);
            _adjacent[source].Remove(target);
            return true;
        }

        public bool ContainsVertex(TVertex vertex) => _adjacent.ContainsKey(vertex);

        public int InDegrees(TVertex target) => _adjacent[target].Count;

        public IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)> OutEdges(TVertex source) => _adjacent[source].Select(kv => (source, kv.Key, kv.Value));

        public int OutDegrees(TVertex source) => _adjacent[source].Count;

        public IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)> InEdges(TVertex target) => _adjacent[target].Select(kv => (kv.Key, target, kv.Value));

        IEnumerable<(TVertex Source, TVertex Target)> IImplicitGraph<TVertex>.OutEdges(TVertex source) => OutEdges(source).Select(edge => (edge.Source, edge.Target));

        IEnumerable<(TVertex Source, TVertex Target)> IExplicitGraph<TVertex>.InEdges(TVertex target) => InEdges(target).Select(edge => (edge.Source, edge.Target));
    }
}

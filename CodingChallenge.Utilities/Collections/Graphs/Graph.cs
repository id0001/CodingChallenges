namespace CodingChallenge.Utilities.Collections.Graphs
{
    public static class Graph
    {
        public static ImplicitGraph<TVertex> Implicit<TVertex>(Func<TVertex, IEnumerable<(TVertex Source, TVertex Target)>> outEdges)
            where TVertex : notnull, IEquatable<TVertex>
            => new ImplicitGraph<TVertex>(outEdges);

        public static WeightedImplicitGraph<TVertex, TWeight> Implicit<TVertex, TWeight>(Func<TVertex, IEnumerable<(TVertex Source, TVertex Target, TWeight Weight)>> outEdges)
            where TVertex : notnull, IEquatable<TVertex>
            where TWeight : notnull
            => new WeightedImplicitGraph<TVertex, TWeight>(outEdges);
    }


    public class Graph<TVertex> : IExplicitGraph<TVertex>, IMutableGraph<TVertex>
        where TVertex : notnull, IEquatable<TVertex>
    {
        private readonly Dictionary<TVertex, HashSet<TVertex>> _adjacent = [];

        public IReadOnlySet<TVertex> Vertices => _adjacent.Keys.ToHashSet();

        public bool AddEdge(TVertex source, TVertex target)
        {
            AddVertex(source);
            AddVertex(target);

            if (_adjacent[source].Contains(target))
                return false;

            _adjacent[source].Add(target);
            _adjacent[target].Add(source);
            return true;
        }

        public bool AddVertex(TVertex vertex) => _adjacent.TryAdd(vertex, []);

        public bool RemoveEdge(TVertex source, TVertex target)
        {
            if (!ContainsVertex(source) || !ContainsVertex(target))
                return false;

            _adjacent[target].Remove(source);
            _adjacent[source].Remove(target);
            return true;
        }

        public bool RemoveVertex(TVertex vertex)
        {
            if (!ContainsVertex(vertex))
                return false;

            foreach (var target in _adjacent[vertex])
                _adjacent[target].Remove(vertex);

            _adjacent.Remove(vertex);
            return true;
        }

        public bool ContainsVertex(TVertex vertex) => _adjacent.ContainsKey(vertex);

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (!ContainsVertex(source) || !ContainsVertex(target))
                return false;

            return _adjacent[source].Contains(target);
        }

        public int InDegrees(TVertex target) => _adjacent[target].Count;

        public IEnumerable<(TVertex Source, TVertex Target)> InEdges(TVertex target) => _adjacent[target].Select(source => (source, target));

        public int OutDegrees(TVertex source) => _adjacent[source].Count;

        public IEnumerable<(TVertex Source, TVertex Target)> OutEdges(TVertex source) => _adjacent[source].Select(target => (source, target));
    }
}

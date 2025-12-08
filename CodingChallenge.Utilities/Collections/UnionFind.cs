namespace CodingChallenge.Utilities.Collections
{
    public class UnionFind<T>
        where T : notnull
    {
        private readonly IEqualityComparer<T> _comparer;
        private readonly Dictionary<T, T> _parent;
        private readonly Dictionary<T, int> _rank;

        public UnionFind() : this(Enumerable.Empty<T>())
        {
        }

        public UnionFind(IEnumerable<T> source) : this(source, null)
        {
        }

        public UnionFind(IEqualityComparer<T> comparer) : this(Enumerable.Empty<T>(), comparer)
        {
        }

        public UnionFind(IEnumerable<T> source, IEqualityComparer<T>? comparer)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
            _parent = new(_comparer);
            _rank = new(_comparer);

            foreach (var item in source)
                MakeSet(item);
        }

        public bool MakeSet(T value)
        {
            if (_parent.ContainsKey(value))
                return false;

            _parent.Add(value, value);
            _rank.Add(value, 0);
            return true;
        }

        public T Find(T item)
        {
            if (!_parent.ContainsKey(item))
                throw new ArgumentException("Item not found in disjoint set.");

            if (!_comparer.Equals(_parent[item], item))
                _parent[item] = Find(_parent[item]);

            return _parent[item];
        }

        public bool AreConnected(T a, T b) => _comparer.Equals(Find(a), Find(b));

        public void Union(T a, T b)
        {
            var rootA = Find(a);
            var rootB = Find(b);

            if (_comparer.Equals(rootA, rootB))
                return;

            var rankA = _rank[rootA];
            var rankB = _rank[rootB];

            if (rankA < rankB)
                _parent[rootA] = rootB;
            else if (rankA > rankB)
                _parent[rootB] = rootA;
            else
            {
                _parent[rootB] = rootA;
                _rank[rootA]++;
            }
        }

        public IEnumerable<HashSet<T>> GetAllSets() 
            => _parent.Keys.Select(k => (Root: Find(k), Item: k)).GroupBy(g => g.Root, g => g.Item).Select(g => g.ToHashSet());
    }
}

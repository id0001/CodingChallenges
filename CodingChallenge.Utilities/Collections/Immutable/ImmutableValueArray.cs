using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;

namespace CodingChallenge.Utilities.Collections.Immutable
{
    public readonly struct ImmutableValueArray<T> : IEquatable<ImmutableValueArray<T>>, IReadOnlyList<T>
        where T : IEquatable<T>
    {
        public static ImmutableValueArray<T> Empty { get; } = new(ImmutableArray<T>.Empty);

        public T this[int index] => _values[index];

        public int Count => _values.Length;

        private readonly ImmutableArray<T> _values = ImmutableArray<T>.Empty;

        public ImmutableValueArray(params T[] values)
        {
            _values = ImmutableArray.Create(values);
        }

        public ImmutableValueArray(ImmutableArray<T> values)
        {
            _values = values;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _values)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Equals(ImmutableValueArray<T> other) => other.SequenceEqual(_values);

        public override int GetHashCode()
        {
            var hc = new HashCode();
            foreach(var v in _values)
                hc.Add(v.GetHashCode());

            return hc.ToHashCode();
        }

        public ImmutableValueArray<T> SetItem(int index, T item) => _values.SetItem(index, item);

        public static implicit operator ImmutableValueArray<T>(ImmutableArray<T> value) => new ImmutableValueArray<T>(value);
        public static implicit operator ImmutableArray<T>(ImmutableValueArray<T> value) => value._values;

        private sealed class DebugView(ImmutableValueArray<T> array)
        {
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T[] Items => array.ToArray();
        }
    }
}

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CodingChallenge.Utilities.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    public class Deque<T> : IReadOnlyCollection<T>, IEnumerable<T>
    {
        private const int GrowFactor = 2;

        private T?[] _array;

        private int _head; // The index from which to remove or add if the deque isn't empty.
        private int _tail; // The index from which to remove or add if the deque isn't empty.
        private int _version;

        public Deque()
        {
            _array = [];
            _tail = 0;
            _head = 0;
        }

        public Deque(int capacity)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 0);

            _array = new T[capacity];
            var center = capacity / 2;
            _tail = center;
            _head = center;
        }

        public Deque(IEnumerable<T> backCollection)
        : this(16)
        {
            PushRangeBack(backCollection);
        }

        public Deque(IEnumerable<T> backCollection, IEnumerable<T> frontCollection)
        : this(16)
        {
            PushRangeBack(backCollection);
            PushRangeFront(frontCollection);
        }

        public int Capacity => _array.Length;

        public bool IsEmpty => Count == 0;

        public int Count { get; private set; }

        public void Rotate(int amount)
        {
            if (amount == 0)
                return;

            if (amount > 0)
                for (var i = 0; i < amount; i++)
                    PushFront(PopBack());
            else
                for (var i = 0; i > amount; i--)
                    PushBack(PopFront());
        }

        public void PushFront(T item)
        {
            var insertIndex = IsEmpty ? _head : _head - 1;

            if (insertIndex < 0)
            {
                Rebalance();
                insertIndex = IsEmpty ? _head : _head - 1;
            }

            _array[insertIndex] = item;
            _head = insertIndex;
            Count++;
            _version++;
        }

        public void PushBack(T item)
        {
            var insertIndex = IsEmpty ? _tail : _tail + 1;

            if (insertIndex == _array.Length)
            {
                Rebalance();
                insertIndex = IsEmpty ? _tail : _tail + 1;
            }

            _array[insertIndex] = item;
            _tail = insertIndex;
            Count++;
            _version++;
        }

        public void PushRangeFront(IEnumerable<T> items)
        {
            foreach (var item in items)
                PushFront(item);
        }

        public void PushRangeBack(IEnumerable<T> items)
        {
            foreach (var item in items)
                PushBack(item);
        }

        public void Clear()
        {
            if (Count != 0)
            {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                    Array.Clear(_array, _head, Count);

                Count = 0;
            }

            var center = _array.Length / 2;
            _head = center;
            _tail = center;
            _version++;
        }

        public T PeekFront()
        {
            if (IsEmpty)
                throw new InvalidOperationException(@"The collection is empty");

            return _array[_head]!;
        }

        public T PeekBack()
        {
            if (IsEmpty)
                throw new InvalidOperationException(@"The collection is empty");

            return _array[_tail]!;
        }

        public T PopFront()
        {
            if (IsEmpty)
                throw new InvalidOperationException(@"The collection is empty");

            var removed = _array[_head]!;
            _array[_head] = default;

            Count--;
            if (!IsEmpty)
                _head++;

            _version++;

            return removed;
        }

        public T PopBack()
        {
            if (IsEmpty)
                throw new InvalidOperationException(@"The collection is empty");

            var removed = _array[_tail]!;
            _array[_tail] = default;

            Count--;
            if (!IsEmpty)
                _tail--;

            _version++;

            return removed;
        }

        private void Rebalance()
        {
            var newCapacity = System.Math.Max(8, Count * GrowFactor);
            var oldArray = _array;
            _array = new T[newCapacity];

            var newHead = _array.Length / 2 - Count / 2;
            var newTail = IsEmpty ? newHead : newHead + Count - 1;

            Array.Copy(oldArray, _head, _array, newHead, Count);

            _head = newHead;
            _tail = newTail;

            _version++;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<T>
        {
            private readonly Deque<T> _source;
            private readonly int _version;
            private int _index;
            private T? _currentElement;

            internal Enumerator(Deque<T> source)
            {
                _source = source;
                _version = _source._version;
                _index = -1;
                _currentElement = default;
            }

            public T Current
            {
                get
                {
                    if (_index < 0)
                        ThrowEnumerationNotStartedOrEnded();

                    return _currentElement!;
                }
            }

            object IEnumerator.Current => Current!;

            public void Dispose()
            {
                _index = -2;
                _currentElement = default;
            }

            public bool MoveNext()
            {
                if (_version != _source._version)
                    throw new InvalidOperationException("The collection has been modified.");

                if (_index == -2)
                    return false;

                _index = _index == -1 ? _source._head : _index + 1;

                if (_index == _source._tail + 1)
                {
                    _index = -2;
                    _currentElement = default;
                    return false;
                }

                _currentElement = _source._array[_index];
                return true;
            }

            public void Reset()
            {
                if(_version != _source._version)
                    throw new InvalidOperationException("The collection has been modified.");

                _index = -1;
                _currentElement = default;
            }

            private void ThrowEnumerationNotStartedOrEnded()
            {
                Debug.Assert(_index is -1 or -2);
                throw new InvalidOperationException(
                    _index == -1 ? "Enumeration has not started." : "Enumeration has ended.");
            }
        }

        internal sealed class DequeDebugView
        {
            private readonly Deque<T> _deque;

            public DequeDebugView(Deque<T> deque)
            {
                _deque = deque;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public IEnumerable<T> Items => _deque;
        }
    }
}

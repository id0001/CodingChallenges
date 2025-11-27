using System.Collections;

namespace CodingChallenge.Utilities.Collections
{
    public class CircularBuffer<T> : IEnumerable<T>
    {
        private readonly T[] _array;
        private int _head;       // The index from which to dequeue if the queue isn't empty.
        private int _tail;       // The index at which to enqueue if the queue isn't full.
        private int _count;       // Number of elements

        public CircularBuffer(int maxCapacity)
        {
            _array = new T[maxCapacity];
        }

        public CircularBuffer(IEnumerable<T> source)
        {
            _array = source.ToArray();
            _count = _array.Length;
        }

        public T this[int index]
        {
            get => _array[InternalIndex(index)];
            set => _array[InternalIndex(index)] = value;
        }

        public int Count => _count;

        public int Capacity => _array.Length;

        public void Enqueue(T item)
        {
            _array[_tail] = item;
            MoveNext(ref _tail);

            if (_count == _array.Length)
                MoveNext(ref _head);
            else
                _count++;
        }

        public T Dequeue()
        {
            if (_count == 0)
                throw new InvalidOperationException("Buffer is empty");

            var item = _array[_head];
            MoveNext(ref _head);
            _count--;

            return item;
        }

        public bool TryDequeue(out T? item)
        {
            item = default;

            if (_count == 0)
                return false;

            item = _array[_head];
            MoveNext(ref _head);
            _count--;

            return true;
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Buffer is empty");

            return _array[_head];
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
                yield return _array[InternalIndex(i)];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void MoveNext(ref int index)
        {
            if (++index == _array.Length)
                index = 0;
        }

        private int InternalIndex(int index)
        {
            return (_head + (index % _count)) % _array.Length;
        }
    }
}

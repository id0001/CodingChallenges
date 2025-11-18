namespace CodingChallenge.Utilities.Collections
{
    public class FlipBuffer<T>
        where T : notnull
    {
        private readonly T[] _array;
        private int _readIndex = 0;
        private int _writeIndex = 1;

        public FlipBuffer(T read, T write) => _array = [read, write];

        public T Read => _array[_readIndex];

        public T Write => _array[_writeIndex];

        public void Flip()
        {
            _readIndex = _readIndex == 1 ? 0 : 1;
            _writeIndex = _writeIndex == 1 ? 0 : 1;
        }
    }
}

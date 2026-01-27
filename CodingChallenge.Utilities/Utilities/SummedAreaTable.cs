using System.Numerics;

namespace CodingChallenge.Utilities.Utilities
{
    /// <summary>
    ///     https://en.wikipedia.org/wiki/Summed-area_table
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record SummedAreaTable<T> where T : IBinaryInteger<T>
    {
        private readonly Grid2<T> _aux;

        public SummedAreaTable(Grid2<T> sourceTable)
        {
            _aux = SummedAreaTable<T>.Preprocess(sourceTable);
        }

        public T SumQuery(int x, int y, int width, int height)
        {
            var brx = x + width - 1;
            var bry = y + height - 1;

            var res = _aux[bry, brx];

            if (y > 0)
                res -= _aux[y - 1, brx];

            if (x > 0)
                res -= _aux[bry, x - 1];

            if (x > 0 && y > 0)
                res += _aux[y - 1, x - 1];

            return res;
        }

        private static Grid2<T> Preprocess(Grid2<T> source)
        {
            var aux = new Grid2<T>(source.RowCount, source.ColumnCount);

            for (var x = 0; x < source.ColumnCount; x++)
                aux[0, x] = source[0, x];

            for (var y = 1; y < source.RowCount; y++)
                for (var x = 0; x < source.ColumnCount; x++)
                    aux[y, x] = source[y, x] + aux[y - 1, x];

            for (var y = 0; y < source.RowCount; y++)
                for (var x = 1; x < source.ColumnCount; x++)
                    aux[y, x] += aux[y, x - 1];

            return aux;
        }
    }
}

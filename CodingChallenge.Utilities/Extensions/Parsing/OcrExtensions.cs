using CodingChallenge.Utilities.Parsing;
using System.Text;

namespace CodingChallenge.Utilities.Extensions
{
    public static class OcrExtensions
    {
        extension(string source)
        {
            public string Ocr()
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);

                return new Ocr().Parse(source);
            }
        }

        extension(bool[,] source)
        {
            public string Ocr()
            {
                ArgumentNullException.ThrowIfNull(source);

                var sb = new StringBuilder();
                for (var y = 0; y < source.GetLength(0); y++)
                {
                    for (var x = 0; x < source.GetLength(1); x++) sb.Append(source[y, x] ? '#' : '.');

                    sb.AppendLine();
                }

                return Ocr(sb.ToString());
            }
        }

        extension(Grid2<bool> source)
        {
            public string Ocr()
            {
                ArgumentNullException.ThrowIfNull(source);

                var sb = new StringBuilder();
                for (var y = 0; y < source.Rows; y++)
                {
                    for (var x = 0; x < source.Columns; x++) sb.Append(source[y, x] ? '#' : '.');

                    sb.AppendLine();
                }

                return Ocr(sb.ToString());
            }
        }
    }
}

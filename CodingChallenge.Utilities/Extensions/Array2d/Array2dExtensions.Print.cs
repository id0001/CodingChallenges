using Spectre.Console;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class Array2dExtensions
    {
        extension<TSource>(Grid2<TSource> source)
        {
            public void SpectrePrint(Func<Point2, TSource, Text> selector)
            {
                for (var y = 0; y < source.Height; y++)
                {
                    for (var x = 0; x < source.Width; x++)
                    {
                        AnsiConsole.Write(selector(new Point2(x, y), source[y, x]));
                    }

                    AnsiConsole.WriteLine();
                }
            }
        }
    }
}

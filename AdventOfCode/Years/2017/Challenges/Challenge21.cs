using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Immutable;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(21)]
public class Challenge21
{
    private const string Start = @".#...####";

    [Part(1, "136")]
    public string Part1(string input)
    {
        var enhancements = input.Lines(ParseLine).ToList();

        var pattern = Start.ToCharArray();
        for (var i = 0; i < 5; i++)
            pattern = Enhance(pattern, enhancements);

        return pattern.Count(c => c == '#').ToString();
    }

    [Part(2, "1911767")]
    public string Part2(string input)
    {
        var enhancements = input.Lines(ParseLine).ToList();

        var pattern = Start.ToCharArray();
        for (var i = 0; i < 18; i++)
            pattern = Enhance(pattern, enhancements);

        return pattern.Count(c => c == '#').ToString();
    }

    private static char[] Enhance(char[] sequence, IEnumerable<Enhancement> enhancements) => sequence.Length switch
    {
        4 or 9 => enhancements.First(e => e.Pattern.IsMatch(sequence)).Output,
        _ => Stitch([.. Subdivide(sequence, sequence.Length % 2 == 0 ? 2 : 3).Select(s => Enhance(s, enhancements))]),
    };

    private static IEnumerable<char[]> Subdivide(char[] sequence, int targetWidth)
    {
        int currentWidth = (int)Math.Sqrt(sequence.Length); // get the width and height of the square.

        for (var chunkY = 0; chunkY < currentWidth / targetWidth; chunkY++)
        {
            for (var chunkX = 0; chunkX < currentWidth / targetWidth; chunkX++)
            {
                var result = new char[targetWidth * targetWidth];

                for (var y = 0; y < targetWidth; y++)
                {
                    for (var x = 0; x < targetWidth; x++)
                    {
                        var i = Point2.ToIndex(chunkX * targetWidth + x, chunkY * targetWidth + y, currentWidth);
                        result[Point2.ToIndex(x, y, targetWidth)] = sequence[i];
                    }
                }

                yield return result;
            }
        }
    }

    private static char[] Stitch(char[][] subdivision)
    {
        var chunkWidth = (int)Math.Sqrt(subdivision.Length);
        var width = (int)Math.Sqrt(subdivision[0].Length);

        var size = chunkWidth * width;
        var result = new char[size * size];

        for (var chunkY = 0; chunkY < chunkWidth; chunkY++)
        {
            for (var chunkX = 0; chunkX < chunkWidth; chunkX++)
            {
                for (var y = 0; y < width; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var chunkIndex = Point2.ToIndex(chunkX, chunkY, chunkWidth);
                        var index = Point2.ToIndex(x, y, width);
                        result[Point2.ToIndex(chunkX * width + x, chunkY * width + y, chunkWidth * width)] = subdivision[chunkIndex][index];
                    }
                }
            }
        }

        return result;
    }

    private static Enhancement ParseLine(string line)
    {
        var (input, output) = line.SplitBy("=>").Transform(parts => (parts.First().Replace("/", string.Empty), parts.Second().Replace("/", string.Empty)));
        return new Enhancement(new Pattern(input.ToCharArray()), output.ToCharArray());
    }

    private record Enhancement(Pattern Pattern, char[] Output);

    private record Pattern
    {
        private readonly HashSet<ImmutableValueArray<char>> _variations = [];

        public Pattern(ImmutableValueArray<char> value)
        {
            _variations.Add(value);

            var hflip = FlipHorizontal(value);
            var vflip = FlipVertical(value);

            _variations.Add(hflip);
            _variations.Add(vflip);

            for (var i = 0; i < 3; i++)
            {
                value = Rotate90(value);
                hflip = Rotate90(hflip);
                vflip = Rotate90(vflip);

                _variations.Add(vflip);
                _variations.Add(hflip);
                _variations.Add(value);
            }
        }

        public bool IsMatch(char[] value) => _variations.Contains(value);

        private static ImmutableValueArray<char> FlipHorizontal(ImmutableValueArray<char> value)
        {
            var width = value.Count == 4 ? 2 : 3;
            var newValue = new char[value.Count];

            for (var i = 0; i < value.Count; i++)
                newValue[i.ToPoint2(width).GridFlip(FlipDirection.Horizontal, width, width).ToIndex(width)] = value[i];

            return new ImmutableValueArray<char>(newValue);
        }

        private static ImmutableValueArray<char> FlipVertical(ImmutableValueArray<char> value)
        {
            var width = value.Count == 4 ? 2 : 3;
            var newValue = new char[value.Count];

            for (var i = 0; i < value.Count; i++)
                newValue[i.ToPoint2(width).GridFlip(FlipDirection.Vertical, width, width).ToIndex(width)] = value[i];

            return new ImmutableValueArray<char>(newValue);
        }

        private static ImmutableValueArray<char> Rotate90(ImmutableValueArray<char> value)
        {
            var width = value.Count == 4 ? 2 : 3;
            var newValue = new char[value.Count];

            for (var i = 0; i < value.Count; i++)
                newValue[i.ToPoint2(width).GridRotate(Rotation.Degree90, width, width).ToIndex(width)] = value[i];

            return new ImmutableValueArray<char>(newValue);
        }
    }
}

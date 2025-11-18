using CodingChallenge.Utilities.Resources;

namespace CodingChallenge.Utilities.Parsing
{
    public class Ocr
    {
        private const string LargeLetterSequence = "ABCEFGHJKLNPRXZ";
        private const string LargeAlphabetResourceName = "CodingChallenge.Utilities.Resources.aoc_alphabet_l.txt";

        private const string SmallLetterSequence = "ABCEFGHIJKLOPRSUYZ";
        private const string SmallAlphabetResourceName = "CodingChallenge.Utilities.Resources.aoc_alphabet_s.txt";

        private static string[] TrimLines(string input)
        {
            var vtrimmed = input
                .Split(Environment.NewLine)
                .Where(line => !line.All(c => c == '.'))
                .ToArray();

            var left = Enumerable.Range(0, vtrimmed[0].Length).First(x => !vtrimmed.All(line => line[x] == '.'));
            var right = Enumerable.Range(0, vtrimmed[0].Length).Last(x => !vtrimmed.All(line => line[x] == '.')) + 1;

            return vtrimmed.Select(line => line[left..right]).ToArray();
        }

        public string Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            var lines = TrimLines(input);
            if (!lines.All(x => x.Length == lines[0].Length))
                throw new ArgumentException("All lines must be the same length", nameof(input));

            return new string(ExtractLetters(GetAlphabetLookup(), lines).ToArray());
        }

        private static Dictionary<string, char> GetAlphabetLookup() => GetSmallLetters().Concat(GetLargeLetters())
            .ToDictionary(kv => kv.Pattern, kv => kv.Letter);

        private static IEnumerable<(char Letter, string Pattern)> GetSmallLetters() =>
            ExtractAlphabetFromResource(SmallLetterSequence, SmallAlphabetResourceName);

        private static IEnumerable<(char Letter, string Pattern)> GetLargeLetters() =>
            ExtractAlphabetFromResource(LargeLetterSequence, LargeAlphabetResourceName);

        private static IEnumerable<(char Letter, string Pattern)> ExtractAlphabetFromResource(string letterSequence,
            string resourceName)
        {
            var lines = ResourceHelper.Read(resourceName).Split(Environment.NewLine);
            int[] xstart =
            [
                0, .. Enumerable.Range(0, lines[0].Length).Where(x => lines.All(line => line[x] == '.')).Select(x => x + 1)
            ];

            for (var i = 0; i < xstart.Length; i++)
                if (i == xstart.Length - 1)
                {
                    var i1 = i;
                    yield return (letterSequence[i], Flatten(lines.Select(line => line[xstart[i1]..])));
                }
                else
                {
                    var i1 = i;
                    yield return (letterSequence[i], Flatten(lines.Select(line => line[xstart[i1]..(xstart[i1 + 1] - 1)])));
                }
        }

        private static IEnumerable<char> ExtractLetters(Dictionary<string, char> lookup, string[] lines)
        {
            var currentLetter = new string[lines.Length];

            for (var x = 0; x < lines[0].Length; x++)
            {
                var scanLine = GetVerticalLine(lines, x);
                if (scanLine.All(c => c == '.'))
                    continue; // Ignore empty space

                for (var y = 0; y < scanLine.Length; y++)
                    currentLetter[y] += scanLine[y];

                if (lookup.TryGetValue(Flatten(currentLetter), out var letter))
                {
                    yield return letter;
                    currentLetter = new string[lines.Length];
                }
            }

            if (lookup.TryGetValue(Flatten(currentLetter), out var lastLetter))
                yield return lastLetter;
        }

        private static string GetVerticalLine(string[] lines, int x) => new(lines.Select(line => line[x]).ToArray());

        private static string Flatten(IEnumerable<string> lines) => lines.Aggregate(string.Concat);
    }
}

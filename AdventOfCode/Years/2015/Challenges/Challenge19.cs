using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(19)]
public class Challenge19
{
    [Part(1, "518")]
    public string Part1(string input)
    {
        var (replacements, sequence) = Parse(input);

        var set = new HashSet<string>();
        foreach (var key in replacements.Keys)
        {
            foreach (var repl in replacements[key])
            {
                for (var i = 0; i < sequence.Length; i++)
                {
                    if (sequence[i] == key)
                        set.Add(string.Concat(sequence[..i]) + repl + string.Concat(sequence[(i + 1)..]));
                }
            }
        }

        return set.Count.ToString();
    }

    [Part(2, "200")]
    public string Part2(string input)
    {
        var (replacements, sequence) = Parse(input);

        var total = sequence.Length;
        var arrn = sequence.Count(x => x is "Ar" or "Rn");
        var y = sequence.Count(x => x == "Y");

        return (total - arrn - 2 * y - 1).ToString();
    }

    public static (Dictionary<string, List<string>> Replacements, string[] Sequence) Parse(string input) => input
            .Paragraphs()
            .Transform(parts => (
                parts
                    .First()
                    .Lines()
                    .Select(x => x.SplitBy("=>"))
                    .GroupBy(x => x.First(), x => x.Second())
                    .ToDictionary(kv => kv.Key, kv => kv.ToList()),
                ToSequence(parts.Second()).ToArray()
                ));

    public static IEnumerable<string> ToSequence(string text)
    {
        foreach (var w in text.Windowed(2))
        {
            if (char.IsLower(w[0]))
                continue;

            if (char.IsUpper(w[1]))
                yield return w[0].ToString();
            else
                yield return w.AsString();
        }
    }
}
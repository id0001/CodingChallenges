using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(14)]
public class Challenge14
{
    [Part(1, "2655")]
    public string Part1(string input) => input
        .Lines(ParseLine).Select(r => r.DistanceTraveledAfter(2503)).Max().ToString();

    [Part(2, "1059")]
    public string Part2(string input)
    {
        var reindeer = input.Lines(ParseLine).ToList();

        var score = reindeer.ToDictionary(kv => kv, _ => (Score: 0, Distance: 0));
        for (var s = 1; s <= 2503; s++)
        {
            foreach (var deer in reindeer)
                score[deer] = score[deer] with { Distance = deer.DistanceTraveledAfter(s) };

            var max = score.Values.MaxBy(x => x.Distance);
            foreach (var deer in reindeer.Where(x => score[x].Distance == max.Distance))
                score[deer] = score[deer] with { Score = score[deer].Score + 1 };
        }

        return score.Values.Select(x => x.Score).Max().ToString();
    }

    private static Reindeer ParseLine(string line) => line
        .Extract<int, int, int>(@"\w+ can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds\.")
        .Transform(x => new Reindeer(x.First, x.Second, x.Third));

    private readonly record struct Reindeer(int SpeedPerSecond, int SecondsBeforeReset, int Rest)
    {
        public int DistanceTraveledAfter(int seconds)
        {
            var fullCycles = seconds / (SecondsBeforeReset + Rest);
            var remainder = seconds - fullCycles * (SecondsBeforeReset + Rest);
            var distance = fullCycles * SpeedPerSecond * SecondsBeforeReset;
            distance += Math.Min(remainder, SecondsBeforeReset) * SpeedPerSecond;
            return distance;
        }
    }
}

using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using CodingChallenge.Utilities.Mathematics;

namespace EverybodyCodes2024.Challenges;

[Challenge(7)]
public class Quest07
{
    [Part(1)]
    public string Part1(string input)
    {
        return input
            .Lines(line => line.SplitBy(":")
            .Transform(p => (
                Id: p.First()[0],
                p.Second()
                    .SplitBy(",")
                    .Cycle()
                    .Take(10)
                    .Aggregate((Score: 0, Value: 10), Process)
                    .Score)))
            .OrderByDescending(x => x.Score)
            .Select(x => x.Id)
            .AsString();
    }

    [Part(2)]
    public string Part2(string input)
    {
        var track = ParseTrack("S-=++=-==++=++=-=+=-=+=+=--=-=++=-==++=-+=-=+=-=+=+=++=-+==++=++=-=-=--\r\n-                                                                     -\r\n=                                                                     =\r\n+                                                                     +\r\n=                                                                     +\r\n+                                                                     =\r\n=                                                                     =\r\n-                                                                     -\r\n--==++++==+=+++-=+=-=+=-+-=+-=+-=+=-=+=--=+++=++=+++==++==--=+=++==+++-").ToList();
        
        return input
            .Lines(line => line.SplitBy(":")
            .Transform(p => (
                Id: p.First()[0],
                p.Second()
                    .SplitBy(",")
                    .Select(x => x[0])
                    .Cycle()
                    .Zip(track.Cycle())
                    .Take(10 * track.Count)
                    .Aggregate((Score: 0, Value: 10), Process2)
                    .Score)))
            .OrderByDescending(x => x.Score)
            .Select(x => x.Id)
            .AsString();
    }

    [Part(3)]
    public string Part3(string input)
    {
        var track = ParseTrack("S+= +=-== +=++=     =+=+=--=    =-= ++=     +=-  =+=++=-+==+ =++=-=-=--\r\n- + +   + =   =     =      =   == = - -     - =  =         =-=        -\r\n= + + +-- =-= ==-==-= --++ +  == == = +     - =  =    ==++=    =++=-=++\r\n+ + + =     +         =  + + == == ++ =     = =  ==   =   = =++=\r\n= = + + +== +==     =++ == =+=  =  +  +==-=++ =   =++ --= + =\r\n+ ==- = + =   = =+= =   =       ++--          +     =   = = =--= ==++==\r\n=     ==- ==+-- = = = ++= +=--      ==+ ==--= +--+=-= ==- ==   =+=    =\r\n-               = = = =   +  +  ==+ = = +   =        ++    =          -\r\n-               = + + =   +  -  = + = = +   =        +     =          -\r\n--==++++==+=+++-= =-= =-+-=  =+-= =-= =--   +=++=+++==     -=+=++==+++-").ToList();

        var enemy = input[3..].SplitBy(",").Select(x => x[0]).Cycle();
        var score = enemy.Zip(track).Take(track.Count * 2024).Aggregate((Score: 0, Value: 10), Process2).Score;

        var options = new char[] { '+', '+', '+', '+', '+', '-', '-', '-', '=', '=', '=' };
        int count = 0;

        var d = new Dictionary<char, int>
        {
            ['+'] = 5,
            ['-'] = 3,
            ['='] = 3
        };

        foreach(var combination in GeneratePermutation(d, new char[11], 0))
        {
            if (combination.Zip(track).Take(track.Count * 2024).Aggregate((Score: 0, Value: 10), Process2).Score > score)
                count++;
        }

        return count.ToString();
    }

    private static (int, int) Process((int Score, int Value) accumulator, string action) => action switch
    {
        "+" => (accumulator.Score + accumulator.Value + 1, accumulator.Value + 1),
        "-" => (accumulator.Score + accumulator.Value - 1, accumulator.Value - 1),
        _ => (accumulator.Score + accumulator.Value, accumulator.Value)
    };

    private static (int, int) Process2((int Score, int Value) accumulator, (char Self, char Track) action) => action switch
    {
        (_, '+') => (accumulator.Score + accumulator.Value + 1, accumulator.Value + 1),
        (_, '-') => (accumulator.Score + accumulator.Value - 1, accumulator.Value - 1),
        ('+', _) => (accumulator.Score + accumulator.Value + 1, accumulator.Value + 1),
        ('-', _) => (accumulator.Score + accumulator.Value - 1, accumulator.Value - 1),
        _ => (accumulator.Score + accumulator.Value, accumulator.Value)
    };

    private IEnumerable<char> ParseTrack(string input)
    {
        var width = input.Lines(x => x.Length).Max();
        var normalized = string.Join(Environment.NewLine, input.Lines(line => line.PadRight(width)));

        var grid = normalized.ToGrid();
        Pose2 tracker = new Pose2(new Point2(1,0), Face.Right);
        yield return grid[tracker.Position];
        tracker = tracker.Step();

        while (tracker.Position != new Point2(1, 0))
        {
            yield return grid[tracker.Position];

            if(!grid.Bounds.Contains(tracker.Ahead) || grid[tracker.Ahead] == ' ')
            {
                if (grid.Bounds.Contains(tracker.Left) && grid[tracker.Left] != ' ')
                    tracker = tracker.TurnLeft();
                else
                    tracker = tracker.TurnRight();
            }

            tracker = tracker.Step();
        }
    }

    private static IEnumerable<char[]> GeneratePermutation(Dictionary<char, int> counts, char[] buffer, int index)
    {
        if(index == buffer.Length)
        {
            var array = new char[buffer.Length];
            Array.Copy(buffer, array, buffer.Length);
            yield return array;
            yield break;
        }

        foreach(var key in counts.Keys)
        {
            if (counts[key] == 0)
                continue;

            counts[key]--;
            buffer[index] = key;

            foreach(var p in GeneratePermutation(counts, buffer, index + 1))
            {
                yield return p;
            }
            counts[key]++;
        }
    }
}

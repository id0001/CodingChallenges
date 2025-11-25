using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2015.Challenges;

[Challenge(18)]
public class Challenge18
{
    [Part(1, "768")]
    public string Part1(string input)
    {
        var read = input.ToGrid();
        var write = new Grid2<char>(100, 100);

        for (var i = 0; i < 100; i++)
        {
            foreach (var p in read.Keys)
            {
                var onCount = p
                    .GetNeighbors(true)
                    .Where(read.Bounds.Contains)
                    .Count(n => read[n] == '#');

                write[p] = read[p] == '#'
                    ? onCount == 2 || onCount == 3 ? '#' : '.'
                    : onCount == 3 ? '#' : '.';
            }

            (read, write) = (write, read);
        }

        return Enumerable.Range(0, 100 * 100).Count(i => read[i / 100, i % 100] == '#').ToString();
    }

    [Part(2, "781")]
    public string Part2(string input)
    {
        var read = input.ToGrid();
        var write = new Grid2<char>(100, 100);

        for (var i = 0; i < 100; i++)
        {
            foreach (var p in read.Keys)
            {
                var onCount = p
                    .GetNeighbors(true)
                    .Where(read.Bounds.Contains)
                    .Count(n => read[n] == '#');

                write[p] = read[p] == '#'
                    ? onCount == 2 || onCount == 3 ? '#' : '.'
                    : onCount == 3 ? '#' : '.';
            }

            write[0, 0] = '#';
            write[0, 99] = '#';
            write[99, 0] = '#';
            write[99, 99] = '#';

            (read, write) = (write, read);
        }

        return Enumerable.Range(0, 100 * 100).Count(i => read[i / 100, i % 100] == '#').ToString();
    }
}

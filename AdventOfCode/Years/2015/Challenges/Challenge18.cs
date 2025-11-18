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
        var db = new FlipBuffer<Grid2<char>>(input.ToGrid(), new Grid2<char>(100, 100));

        for (var i = 0; i < 100; i++)
        {
            foreach (var p in db.Read.Keys)
            {
                var onCount = p
                    .GetNeighbors(true)
                    .Where(db.Read.Bounds.Contains)
                    .Count(n => db.Read[n] == '#');

                db.Write[p] = db.Read[p] == '#'
                    ? onCount == 2 || onCount == 3 ? '#' : '.'
                    : onCount == 3 ? '#' : '.';
            }

            db.Flip();
        }

        return Enumerable.Range(0, 100 * 100).Count(i => db.Read[i / 100, i % 100] == '#').ToString();
    }

    [Part(2, "781")]
    public string Part2(string input)
    {
        var db = new FlipBuffer<Grid2<char>>(input.ToGrid(), new Grid2<char>(100, 100));

        for (var i = 0; i < 100; i++)
        {
            foreach (var p in db.Read.Keys)
            {
                var onCount = p
                    .GetNeighbors(true)
                    .Where(db.Read.Bounds.Contains)
                    .Count(n => db.Read[n] == '#');

                db.Write[p] = db.Read[p] == '#'
                    ? onCount == 2 || onCount == 3 ? '#' : '.'
                    : onCount == 3 ? '#' : '.';
            }

            db.Write[0, 0] = '#';
            db.Write[0, 99] = '#';
            db.Write[99, 0] = '#';
            db.Write[99, 99] = '#';

            db.Flip();
        }

        return Enumerable.Range(0, 100 * 100).Count(i => db.Read[i / 100, i % 100] == '#').ToString();
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(9)]
public class Challenge09
{
    [Part(1, "412959")]
    public string Part1(string input)
    {
        var (players, points) = input.Extract<int, int>(@"(\d+) players; last marble is worth (\d+) points");
        return Play(players, points).ToString();
    }

    [Part(2, "3333662986")]
    public string Part2(string input)
    {
        var (players, points) = input.Extract<int, int>(@"(\d+) players; last marble is worth (\d+) points");
        return Play(players, points * 100).ToString();
    }

    private static long Play(int playerCount, int lastMarbleWorth)
    {
        var circle = new Deque<long>([0]);
        var scores = new long[playerCount];

        foreach(var m in Enumerable.Range(1, lastMarbleWorth + 1))
        {
            if(m % 23 == 0)
            {
                circle.Rotate(7);
                scores[m % playerCount] += m + circle.PopBack();
                circle.Rotate(-1);
                continue;
            }

            circle.Rotate(-1);
            circle.PushBack(m);
        }

        return scores.Max();
    }
}

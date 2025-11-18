using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace EverybodyCodes2024.Challenges;

[Challenge(1)]
public class Quest01
{
    [Part(1, "1299")]
    public string Part1(string input) => input.Aggregate(0, (sum, enemy) => sum += enemy switch
    {
        'B' => 1,
        'C' => 3,
        _ => 0
    }).ToString();

    [Part(2, "5699")]
    public string Part2(string input) => input.Chunk(2).Aggregate(0, (sum, pair) =>
    {
        var gs = pair.Count(x => x != 'x');
        var potions = Math.Max(0, CountPotionsNeeded(pair[0], gs) + CountPotionsNeeded(pair[1], gs));
        return sum + potions;
    }).ToString();

    [Part(3, "28170")]
    public string Part3(string input) => input.Chunk(3).Aggregate(0, (sum, pair) =>
    {
        var gs = pair.Count(x => x != 'x');
        var potions = Math.Max(0, CountPotionsNeeded(pair[0], gs) + CountPotionsNeeded(pair[1], gs) + CountPotionsNeeded(pair[2], gs));
        return sum + potions;
    }).ToString();

    private int CountPotionsNeeded(char c, int groupSize)
    {
        if (c == 'x')
            return 0;

        return (groupSize - 1) + c switch
        {
            'D' => 5,
            'C' => 3,
            'B' => 1,
            _ => 0
        };
    }
}

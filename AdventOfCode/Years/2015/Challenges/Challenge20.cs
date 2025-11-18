using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;

namespace AdventOfCode2015.Challenges;

[Challenge(20)]
public class Challenge20
{
    [Part(1, "665280")]
    public string Part1(string input)
    {
        int intInput = int.Parse(input);
        var array = new int[intInput / 10];
        for (var elf = 1; elf < intInput / 10; elf++)
        {
            for (var house = elf; house < intInput / 10; house += elf)
                array[house - 1] += elf * 10;
        }

        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] >= intInput)
                return (i + 1).ToString();
        }

        throw new InvalidOperationException();
    }

    [Part(2, "705600")]
    public string Part2(string input)
    {
        int intInput = int.Parse(input);
        var array = new int[intInput / 11];
        for (var elf = 1; elf < intInput / 11; elf++)
        {
            for (var m = 0; m < 50; m++)
            {
                var house = elf + m * elf;
                if (house >= array.Length)
                    break;

                array[house - 1] += elf * 11;
            }
        }

        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] >= intInput)
                return (i + 1).ToString();
        }

        throw new InvalidOperationException();
    }
}

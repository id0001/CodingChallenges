using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2025.Challenges;

[Challenge(6)]
public class Challenge06
{
    [Part(1, "4648618073226")]
    public string Part1(string input)
    {
        var grid = new Grid2<string>(input.Lines(line => line.SplitBy(" ").ToArray()).ToArray().To2dArray());

        long sum = 0;
        for (var x = 0; x < grid.Columns; x++)
        {
            long value = grid[0, x].As<long>();
            switch (grid[grid.Rows - 1, x])
            {
                case "*":
                    for (var y = 1; y < grid.Rows - 1; y++)
                        value *= grid[y, x].As<long>();
                    break;
                case "+":
                    for (var y = 1; y < grid.Rows - 1; y++)
                        value += grid[y, x].As<long>();
                    break;
            }

            sum += value;
        }

        return sum.ToString();
    }

    [Part(2, "7329921182115")]
    public string Part2(string input)
    {
        var grid = input.ToGrid();

        var numbers = new List<long>();
        var sum = 0L;
        for (var x = grid.Columns - 1; x >= 0; x--)
        {
            var number = grid.Column(x).Where(char.IsNumber).AsString().As<long>();
            numbers.Add(number);

            if (grid[grid.Rows - 1, x] != ' ')
            {
                switch (grid[grid.Rows - 1, x])
                {
                    case '*':
                        sum += numbers.Product();
                        break;
                    case '+':
                        sum += numbers.Sum();
                        break;
                }

                numbers.Clear();
                x--;
            }
        }

        return sum.ToString();
    }
}

using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(8)]
public class Challenge08
{
    [Part(1, "106")]
    public string Part1(string input) => input
        .Lines()
        .Aggregate(new Grid2<bool>(6, 50), ParseInstruction)
        .Values
        .Count(x => x).ToString();

    [Part(2, "CFLELOYFCS")]
    public string Part2(string input) => input
        .Lines()
        .Aggregate(new Grid2<bool>(6, 50), ParseInstruction)
        .Ocr();

    private Grid2<bool> ParseInstruction(Grid2<bool> matrix, string instruction)
    {
        if (instruction.StartsWith("rect"))
        {
            var (xlen, ylen) = instruction.Extract<int, int>(@"(\d+)x(\d+)");
            for (var y = 0; y < ylen; y++)
                for (var x = 0; x < xlen; x++)
                    matrix[y, x] = true;
        }
        else if (instruction.StartsWith("rotate row"))
        {
            var (y, c) = instruction.Extract<int, int>(@"y=(\d+) by (\d+)");
            var row = matrix.Row(y).ToArray();
            for (var x = 0; x < row.Length; x++)
                matrix[y, (x + c).Mod(row.Length)] = row[x];
        }
        else if (instruction.StartsWith("rotate column"))
        {
            var (x, c, _) = instruction.Extract(@"x=(\d+) by (\d+)").As<int>();
            var column = matrix.Column(x).ToArray();
            for (var y = 0; y < column.Length; y++)
                matrix[(y + c).Mod(column.Length), x] = column[y];
        }

        return matrix;
    }
}

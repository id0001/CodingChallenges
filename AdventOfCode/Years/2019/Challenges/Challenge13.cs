using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(13)]
public class Challenge13
{
    [Part(1, "309")]
    public string Part1(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();

        var screenBuffer = new Grid2<int>(64, 64);
        var cpu = new Cpu<long>(program);
        while (cpu.Next())
        {
            if (cpu.OutputCount == 3)
            {
                var (x, y, id, _) = cpu.ReadAll();
                screenBuffer[(int)y, (int)x] = (int)id;
            }
        }

        return screenBuffer.Values.Count(x => x == 2).ToString();
    }

    [Part(2, "15410")]
    public string Part2(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();
        program[0] = 2;

        var score = 0;
        var ballX = 0;
        var paddleX = 0;

        var screenBuffer = new Grid2<int>(64, 64);
        var cpu = new Cpu<long>(program);
        while (cpu.Next())
        {
            if (cpu.InputNeeded)
            {
                if (ballX == paddleX)
                    cpu.Write(0);
                else if (ballX < paddleX)
                    cpu.Write(-1);
                else
                    cpu.Write(1);
            }

            if (cpu.OutputCount == 3)
            {
                var (x, y, id, _) = cpu.ReadAll();
                switch (id)
                {
                    case 3:
                        paddleX = (int)x;
                        break;
                    case 4:
                        ballX = (int)x;
                        break;
                    default:
                        break;
                }

                if (x == -1 && y == 0)
                    score = (int)id;
                else
                    screenBuffer[(int)y, (int)x] = (int)id;
            }
        }

        return score.ToString();
    }
}

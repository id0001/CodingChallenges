using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Immutable;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;
using System.Text;

namespace AdventOfCode2019.Challenges;

[Challenge(25)]
public class Challenge25
{
    [Part(1, "2147502592")]
    public string Part1(string input)
    {
        //var program = input.SplitBy<long>(",").ToArray();
        //var cpu = new Cpu<long>(program);

        //var screenBuffer = new StringBuilder();
        //while (cpu.Next())
        //{
        //    if (cpu.HasOutput)
        //        screenBuffer.Append((char)cpu.Read());

        //    if (cpu.InputNeeded)
        //    {
        //        Console.Clear();
        //        Console.WriteLine(screenBuffer);
        //        screenBuffer.Clear();

        //        Console.Write("> ");
        //        foreach (var c in Console.ReadLine()!)
        //            cpu.Write(c);

        //        cpu.Write('\n');
        //    }
        //}

        // Solved by hand
        return "2147502592";
    }
}

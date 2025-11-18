using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(7)]
public class Challenge07
{
    [Part(1, "422858")]
    public string Part1(string input)
    {
        var program = input.SplitBy<int>(",").ToArray();

        var perms = Enumerable.Range(0, 5).Permutations();
        var highest = int.MinValue;
        foreach (var permutation in perms)
        {
            var signal = 0;
            foreach (var phase in permutation)
            {
                var cpu = new Cpu<int>(program);
                cpu.Write(phase, signal);
                while (cpu.Next())
                {
                    if(cpu.HasOutput)
                        signal = cpu.Read();
                }

                if (signal > highest)
                    highest = signal;
            }
        }

        return highest.ToString();
    }

    [Part(2, "14897241")]
    public string Part2(string input)
    {
        var program = input.SplitBy<int>(",").ToArray();
        var perms = Enumerable.Range(5, 5).Permutations();

        var highest = int.MinValue;
        foreach (var permutation in perms)
        {
            var amps = new Queue<Cpu<int>>();
            foreach (var phase in permutation)
            {
                var amp = new Cpu<int>(program);
                amp.Write(phase);
                amps.Enqueue(amp);
            }

            var current = amps.Dequeue();
            current.Write(0);
            var lastSignal = -1;
            while (current.Next())
            {
                if (current.IsHalted)
                {
                    if (amps.Count == 0)
                        break;

                    current = amps.Dequeue();
                    continue;
                }

                if (current.InputNeeded)
                {
                    amps.Enqueue(current);
                    current = amps.Dequeue();
                    continue;
                }

                if (current.HasOutput)
                {
                    lastSignal = current.Read();
                    if (amps.Count > 0)
                        amps.Peek().Write(lastSignal);
                }
            }

            if (lastSignal > highest)
                highest = lastSignal;
        }

        return highest.ToString();
    }
}

using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2019.Challenges;

[Challenge(23)]
public class Challenge23
{
    [Part(1, "16685")]
    public string Part1(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();
        var cpus = new Cpu<long>[50];
        var packetQueues = new Queue<long>[50];

        for (var i = 0; i < 50; i++)
        {
            packetQueues[i] = new Queue<long>();
            cpus[i] = new Cpu<long>(program);
            cpus[i].Write(i);
        }

        while (true)
        {
            for (var i = 0; i < 50; i++)
            {
                if (cpus[i].Next())
                {
                    if (cpus[i].InputNeeded)
                    {
                        if (packetQueues[i].Count > 0)
                            cpus[i].Write(packetQueues[i].Dequeue());
                        else
                            cpus[i].Write(-1);
                    }

                    if (cpus[i].OutputCount >= 3)
                    {
                        var dest = (int)cpus[i].Read();
                        var x = cpus[i].Read();
                        var y = cpus[i].Read();

                        if (dest == 255)
                            return y.ToString();

                        packetQueues[dest].Enqueue(x);
                        packetQueues[dest].Enqueue(y);
                    }
                }
            }
        }

        throw new InvalidOperationException();
    }

    [Part(2, "11048")]
    public string Part2(string input)
    {
        var program = input.SplitBy<long>(",").ToArray();
        var cpus = new Cpu<long>[50];
        var packetQueues = new Queue<long>[50];
        var readIdle = new bool[50];

        for (var i = 0; i < 50; i++)
        {
            packetQueues[i] = new Queue<long>();
            cpus[i] = new Cpu<long>(program);
            cpus[i].Write(i);
        }

        long natX = -1;
        long natY = -1;
        long lastY = -1;

        while (true)
        {
            for (var i = 0; i < 50; i++)
            {
                if (cpus[i].Next())
                {
                    if (cpus[i].InputNeeded)
                    {
                        if (packetQueues[i].Count > 0)
                        {
                            cpus[i].Write(packetQueues[i].Dequeue());
                            readIdle[i] = false;
                        }
                        else
                        {
                            cpus[i].Write(-1);
                            readIdle[i] = true;
                        }
                    }

                    if (cpus[i].OutputCount >= 3)
                    {
                        var dest = (int)cpus[i].Read();
                        var x = cpus[i].Read();
                        var y = cpus[i].Read();

                        if (dest == 255)
                        {
                            natX = x;
                            natY = y;
                            continue;
                        }

                        packetQueues[dest].Enqueue(x);
                        packetQueues[dest].Enqueue(y);
                    }
                }

                if (packetQueues.Any(p => p.Count != 0) || !readIdle.All(i => i) || natY < 0) continue;

                if (lastY == natY) return natY.ToString();

                packetQueues[0].Enqueue(natX);
                packetQueues[0].Enqueue(natY);
                lastY = natY;
            }
        }

        throw new InvalidOperationException();
    }
}

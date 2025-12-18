using CodingChallenge.Utilities.Assembly;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2017.Challenges;

[Challenge(18)]
public class Challenge18
{
    [Part(1, "2951")]
    public string Part1(string input)
    {
        var program = input.Lines(ParseLine).ToArray();
        var registers = new Dictionary<string, long>();

        long freq = 0;
        for (var ip = 0; ip < program.Length && ip >= 0;)
        {
            // Resolve argument values
            long aVal = program[ip].Arguments.GetValue(0, r => registers.GetValueOrDefault(r, 0));
            long bVal = program[ip].Arguments.Length > 1 ? program[ip].Arguments.GetValue(1, r => registers.GetValueOrDefault(r, 0)) : 0;

            switch (program[ip])
            {
                case ("set", var args):
                    registers[args[0]] = bVal;
                    ip++;
                    break;
                case ("mul", var args):
                    registers[args[0]] = aVal * bVal;
                    ip++;
                    break;
                case ("add", var args):
                    registers[args[0]] = aVal + bVal;
                    ip++;
                    break;
                case ("mod", var args):
                    registers[args[0]] = aVal % bVal;
                    ip++;
                    break;
                case ("jgz", var args):
                    ip += aVal > 0 ? (int)bVal : 1;
                    break;
                case ("snd", var args):
                    freq = aVal;
                    ip++;
                    break;
                case ("rcv", var args):
                    if (aVal != 0)
                        return freq.ToString();

                    ip++;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        throw new InvalidOperationException();
    }

    [Part(2, "7366")]
    public string Part2(string input)
    {
        var program = input.Lines(ParseLine).ToArray();
        var cpu1 = new Cpu(program, 0);
        var cpu2 = new Cpu(program, 1);
        cpu1.WriteTarget = cpu2;
        cpu2.WriteTarget = cpu1;

        while (true)
        {
            if (cpu1.IsHalted && cpu2.IsHalted)
                break;

            if (cpu1.IsWaiting && cpu2.IsWaiting)
                break;

            cpu1.Next();
            cpu2.Next();
        }

        return cpu2.SendCount.ToString();
    }

    private static Instruction ParseLine(string line) => line
        .SplitBy(" ")
        .Transform(args => new Instruction(args[0], args.Length switch
        {
            3 => Arguments.Parse<long, long>(args[1..]),
            2 => Arguments.Parse<string>(args[1..]),
            _ => throw new NotSupportedException()
        }));

    private class Cpu(Instruction[] program, int p)
    {
        private readonly Queue<long> _messageQueue = new();

        private int _ip = 0;

        public Dictionary<string, long> Registers { get; } = new Dictionary<string, long> { { "p", p } };

        public Cpu? WriteTarget { get; set; }

        public int SendCount { get; private set; }

        public void Write(long value) => _messageQueue.Enqueue(value);

        public bool IsHalted => _ip < 0 || _ip >= program.Length;

        public bool IsWaiting { get; private set; }

        public void Next()
        {
            if (WriteTarget is null)
                throw new InvalidOperationException("Other cpu needed");

            if (IsHalted)
                return;

            // Resolve argument values
            long aVal = program[_ip].Arguments.GetValue(0, r => Registers.GetValueOrDefault(r, 0));
            long bVal = program[_ip].Arguments.Length > 1 ? program[_ip].Arguments.GetValue(1, r => Registers.GetValueOrDefault(r, 0)) : 0;

            switch (program[_ip])
            {
                case ("set", var args):
                    Registers[args[0]] = bVal;
                    _ip++;
                    break;
                case ("mul", var args):
                    Registers[args[0]] = aVal * bVal;
                    _ip++;
                    break;
                case ("add", var args):
                    Registers[args[0]] = aVal + bVal;
                    _ip++;
                    break;
                case ("mod", var args):
                    Registers[args[0]] = aVal % bVal;
                    _ip++;
                    break;
                case ("jgz", var args):
                    _ip += aVal > 0 ? (int)bVal : 1;
                    break;
                case ("snd", var args):
                    WriteTarget.Write(aVal);
                    SendCount++;
                    _ip++;
                    break;
                case ("rcv", var args):
                    if (_messageQueue.Count == 0)
                    {
                        IsWaiting = true;
                        return;
                    }

                    IsWaiting = false;
                    Registers[args[0]] = _messageQueue.Dequeue();
                    _ip++;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}

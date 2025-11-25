using CodingChallenge.Utilities.Extensions;
using System.Numerics;

namespace AdventOfCode2019
{
    public class Cpu<TMemory> where TMemory : IBinaryInteger<TMemory>
    {
        private readonly Queue<TMemory> _inputBuffer = new Queue<TMemory>();
        private readonly Queue<TMemory> _outputBuffer = new Queue<TMemory>();

        private int _ip = 0;
        private TMemory[] _memory = [];
        private TMemory _relativeBase = TMemory.Zero;
        private TMemory[] _program;

        public Cpu(TMemory[] program)
        {
            _program = program;
            _memory = new TMemory[1_000_000];
            Array.Copy(program, _memory, program.Length);
        }

        public bool IsHalted => _ip < 0 || _ip >= _memory.Length;

        public bool InputNeeded { get; private set; }

        public bool HasOutput => _outputBuffer.Count > 0;

        public int OutputCount => _outputBuffer.Count;

        public TMemory ExitCode => _memory[0];

        public void Write(params TMemory[] input)
        {
            foreach (var v in input)
                _inputBuffer.Enqueue(v);
        }

        public TMemory Read() => _outputBuffer.Dequeue();

        public TMemory[] ReadAll()
        {
            TMemory[] buffer = new TMemory[_outputBuffer.Count];
            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = _outputBuffer.Dequeue();

            return buffer;
        }

        public bool Next()
        {
            if (IsHalted)
                return false;

            var opCode = int.CreateChecked(ReadAt(_ip) % TMemory.CreateChecked(100)); // first 2 digits
            _ip = opCode switch
            {
                1 => Add(GetValue(0), GetValue(1), GetAddress(2)),
                2 => Multiply(GetValue(0), GetValue(1), GetAddress(2)),
                3 => Input(GetAddress(0)),
                4 => Output(GetValue(0)),
                5 => JumpIfTrue(GetValue(0), int.CreateChecked(GetValue(1))),
                6 => JumpIfFalse(GetValue(0), int.CreateChecked(GetValue(1))),
                7 => LessThan(GetValue(0), GetValue(1), GetAddress(2)),
                8 => Equal(GetValue(0), GetValue(1), GetAddress(2)),
                9 => UpdateRelativeBase(GetValue(0)),
                99 => -1,
                _ => throw new NotImplementedException()
            };

            return true;
        }

        public TMemory RunTillHalted(params TMemory[] input)
        {
            foreach (var n in input)
                _inputBuffer.Enqueue(n);

            while (!IsHalted)
                Next();

            return ExitCode;
        }

        public void Reset()
        {
            _ip = 0;
            _relativeBase = TMemory.Zero;
            _inputBuffer.Clear();
            _outputBuffer.Clear();

            Array.Copy(_program, _memory, _program.Length);
        }

        private TMemory GetValue(int offset)
        {
            var parameter = ReadAt(_ip + offset + 1);
            var mode = ReadAt(_ip).DigitAt(2 + offset);
            return mode switch
            {
                1 => parameter,
                2 => ReadAt(int.CreateChecked(parameter + _relativeBase)),
                _ => ReadAt(int.CreateChecked(parameter))
            };
        }

        private int GetAddress(int offset)
        {
            var parameter = ReadAt(_ip + offset + 1);
            var mode = ReadAt(_ip).DigitAt(2 + offset);
            return mode switch
            {
                2 => int.CreateChecked(parameter + _relativeBase),
                _ => int.CreateChecked(parameter)
            };
        }

        private TMemory ReadAt(int address) => _memory[address];

        private void WriteAt(int address, TMemory value) => _memory[address] = value;

        private int Add(TMemory a, TMemory b, int dest)
        {
            WriteAt(dest, a + b);
            return _ip + 4;
        }

        private int Multiply(TMemory a, TMemory b, int dest)
        {
            WriteAt(dest, a * b);
            return _ip + 4;
        }

        private int Input(int dest)
        {
            if (_inputBuffer.Count == 0)
            {
                InputNeeded = true;
                return _ip;
            }

            InputNeeded = false;
            WriteAt(dest, _inputBuffer.Dequeue());
            return _ip + 2;
        }

        private int Output(TMemory a)
        {
            _outputBuffer.Enqueue(a);
            return _ip + 2;
        }

        private int JumpIfTrue(TMemory condition, int address)
        {
            if (condition != TMemory.Zero)
                return address;

            return _ip + 3;
        }

        private int JumpIfFalse(TMemory condition, int address)
        {
            if (condition == TMemory.Zero)
                return address;

            return _ip + 3;
        }

        private int LessThan(TMemory a, TMemory b, int address)
        {
            WriteAt(address, a < b ? TMemory.One : TMemory.Zero);
            return _ip + 4;
        }

        private int Equal(TMemory a, TMemory b, int address)
        {
            WriteAt(address, a == b ? TMemory.One : TMemory.Zero);
            return _ip + 4;
        }

        private int UpdateRelativeBase(TMemory a)
        {
            _relativeBase += a;
            return _ip + 2;
        }
    }
}

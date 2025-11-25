using System.Collections.Immutable;
using System.Collections.Specialized;

namespace CodingChallenge.Utilities.Collections.Immutable
{
    public readonly record struct ImmutableBitVector32 : IEquatable<ImmutableBitVector32>
    {
        private readonly BitVector32 _bits;
        private readonly ImmutableList<BitVector32.Section> _sections;

        public ImmutableBitVector32()
        {
            _bits = new BitVector32();
            _sections = ImmutableList<BitVector32.Section>.Empty;
        }

        private ImmutableBitVector32(BitVector32 bits, ImmutableList<BitVector32.Section> sections)
            => (_bits, _sections) = (bits, sections);

        public IReadOnlyList<BitVector32.Section> Sections => _sections;

        public ImmutableBitVector32 AddSection(short maxValue)
        {
            if (_sections.Count == 0)
                return new ImmutableBitVector32(_bits, _sections.Add(BitVector32.CreateSection(maxValue)));

            return new ImmutableBitVector32(_bits, _sections.Add(BitVector32.CreateSection(maxValue, _sections[^1])));
        }

        public ImmutableBitVector32 Set(int sectionIndex, int value)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(sectionIndex, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(sectionIndex, _sections.Count);

            var bits = new BitVector32(_bits);
            bits[_sections[sectionIndex]] = value;

            return new ImmutableBitVector32(bits, _sections);
        }

        public ImmutableBitVector32 Increment(int sectionIndex, int value) => Set(sectionIndex, Get(sectionIndex) + value);

        public ImmutableBitVector32 Decrement(int sectionIndex, int value) => Set(sectionIndex, Get(sectionIndex) - value);

        public int Get(int sectionIndex)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(sectionIndex, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(sectionIndex, _sections.Count);

            return _bits[_sections[sectionIndex]];
        }

        public override int GetHashCode() => _bits.GetHashCode();

        public bool Equals(ImmutableBitVector32 other) => _bits.Equals(other._bits);
    }
}

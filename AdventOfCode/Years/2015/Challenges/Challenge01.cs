using CodingChallenge.Utilities.Attributes;

namespace AdventOfCode2015.Challenges
{
    [Challenge(1)]
    public class Challenge01
    {
        [Part(1, "138")]
        public string Part1(string input) => input.Sum(b => b == '(' ? 1 : -1).ToString();

        [Part(2, "1771")]
        public string Part2(string input)
        {
            var (floor, position) = (0, 0);
            foreach (var c in input)
            {
                position++;
                floor += c == '(' ? 1 : -1;
                if (floor == -1)
                    return position.ToString();
            }

            return string.Empty;
        }
    }
}

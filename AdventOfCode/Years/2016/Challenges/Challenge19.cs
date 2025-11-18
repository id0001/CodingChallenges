using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Extensions;
using System.Xml.Linq;

namespace AdventOfCode2016.Challenges;

[Challenge(19)]
public class Challenge19
{
    [Part(1, "1816277")]
    public string Part1(string input)
    {
        var linkedList = new LinkedList<int>();
        for (var i = 0; i < input.As<int>(); i++)
            linkedList.AddLast(i + 1);

        var current = linkedList.First;
        while (linkedList.Count > 1)
        {
            linkedList.Remove(current!.NextOrFirst()!);
            current = current!.NextOrFirst();
        }

        return linkedList.First!.Value.ToString();
    }

    [Part(2, "1410967")]
    public string Part2(string input)
    {
        LinkedListNode<int> current = null!;

        var count = input.As<int>();
        var linkedList = new LinkedList<int>();
        for (var i = 0; i < count; i++)
        {
            linkedList.AddLast(i + 1);

            if (i == count / 2)
                current = linkedList.Last!;
        }

        while (linkedList.Count > 1)
        {
            var next = current.NextOrFirst();
            linkedList.Remove(current);
            current = linkedList.Count % 2 == 1 ? next! : next.NextOrFirst();
        }

        return linkedList.First!.Value.ToString();
    }
}

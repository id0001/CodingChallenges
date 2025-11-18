using CodingChallenge.Utilities.Attributes;
using System.Text.Json;

namespace AdventOfCode2015.Challenges;

[Challenge(12)]
public class Challenge12
{
    [Part(1, "191164")]
    public string Part1(string input)
    {
        var jsonDoc = JsonDocument.Parse(input);
        var sum = 0;
        var stack = new Stack<JsonElement>([jsonDoc.RootElement]);
        while (stack.Count > 0)
        {
            var current = stack.Pop();

            switch (current.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var child in current.EnumerateObject())
                        stack.Push(child.Value);
                    break;
                case JsonValueKind.Array:
                    foreach (var child in current.EnumerateArray())
                        stack.Push(child);
                    break;
                case JsonValueKind.Number:
                    sum += current.GetInt32();
                    break;
                default:
                    break;
            }
        }

        return sum.ToString();
    }

    [Part(2, "87842")]
    public string Part2(string input)
    {
        var jsonDoc = JsonDocument.Parse(input);
        var sum = 0;
        var queue = new Queue<JsonElement>([jsonDoc.RootElement]);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            switch (current.ValueKind)
            {
                case JsonValueKind.Object:
                    var children = current.EnumerateObject();
                    if (children.Any(c => c.Value.ValueKind == JsonValueKind.String && c.Value.GetString() == "red"))
                        break;

                    foreach (var child in children)
                        queue.Enqueue(child.Value);
                    break;
                case JsonValueKind.Array:
                    foreach (var child in current.EnumerateArray())
                        queue.Enqueue(child);
                    break;
                case JsonValueKind.Number:
                    sum += current.GetInt32();
                    break;
                default:
                    break;
            }
        }

        return sum.ToString();
    }
}

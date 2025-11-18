using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2016.Challenges;

[Challenge(10)]
public class Challenge10
{
    [Part(1, "181")]
    public string Part1(string input)
    {
        var nodes = input.Lines(ParseLine).ToList();

        var compareNodes = nodes.OfType<CompareNode>().ToDictionary(kv => kv.Bot);
        var botMemory = compareNodes.ToDictionary(kv => kv.Key, _ => new List<int>());

        foreach (var inode in nodes.OfType<InputNode>())
            botMemory[inode.Bot].Add(inode.Value);

        while (true)
        {
            foreach (var toProcess in botMemory.Where(kv => kv.Value.Count == 2))
            {
                var cb = compareNodes[toProcess.Key];
                var values = toProcess.Value.Order().ToArray();
                toProcess.Value.Clear();

                if (values[0] == 17 && values[1] == 61)
                    return cb.Bot.ToString(); // Done

                if (cb.LowType == "bot")
                    botMemory[cb.Low].Add(values[0]);

                if (cb.HighType == "bot")
                    botMemory[cb.High].Add(values[1]);
            }
        }
    }

    [Part(2, "12567")]
    public string Part2(string input)
    {
        var nodes = input.Lines(ParseLine).ToList();

        var compareNodes = nodes.OfType<CompareNode>().ToDictionary(kv => kv.Bot);
        var botMemory = compareNodes.ToDictionary(kv => kv.Key, _ => new List<int>());
        var outputs = new int[compareNodes.Values.SelectMany(n => new[] { (Type: n.LowType, Id: n.Low), (Type: n.HighType, Id: n.High) }).Where(n => n.Type == "output").Max(n => n.Id) + 1];

        foreach (var inode in nodes.OfType<InputNode>())
            botMemory[inode.Bot].Add(inode.Value);

        bool nothingProcessed;
        do
        {
            nothingProcessed = true;

            foreach (var toProcess in botMemory.Where(kv => kv.Value.Count == 2))
            {
                nothingProcessed = false;

                var cb = compareNodes[toProcess.Key];
                var values = toProcess.Value.Order().ToArray();
                toProcess.Value.Clear();

                if (cb.LowType == "bot")
                    botMemory[cb.Low].Add(values[0]);
                else
                    outputs[cb.Low] = values[0];

                if (cb.HighType == "bot")
                    botMemory[cb.High].Add(values[1]);
                else
                    outputs[cb.High] = values[1];
            }
        } while (!nothingProcessed);

        return (outputs[0] * outputs[1] * outputs[2]).ToString();
    }

    private static Node ParseLine(string line)
    {
        if (line.TryExtract(@"value (\d+) goes to bot (\d+)", out int value, out int inputBot))
            return new InputNode(inputBot, value);

        if (line.TryExtract(@"bot (\d+) gives low to (output|bot) (\d+) and high to (output|bot) (\d+)", out int bs, out string lowType, out int lowBot, out string highType, out int highBot))
            return new CompareNode(bs, lowType, lowBot, highType, highBot);

        throw new NotImplementedException();
    }

    private abstract record Node(int Bot);
    private sealed record CompareNode(int Bot, string LowType, int Low, string HighType, int High) : Node(Bot);
    private sealed record InputNode(int Bot, int Value) : Node(Bot);
}

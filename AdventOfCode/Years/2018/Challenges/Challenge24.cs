using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Extensions;
using System.Collections;

namespace AdventOfCode2018.Challenges;

[Challenge(24)]
public class Challenge24
{
    [Part(1, "14377")]
    public string Part1(string input)
    {
        var (immuneSystem, infected) = ParseInput(input);

        var winner = Fight(new Army(ArmyType.ImmuneSystem, immuneSystem, 0), new Army(ArmyType.Infected, infected, 0));
        return winner!.UnitsLeft.ToString();
    }

    [Part(2, "6947")]
    public string Part2(string input)
    {
        var (immuneSystem, infected) = ParseInput(input);

        var boost = 0;
        Army? winner;
        for (; ; boost++)
        {
            var army1 = new Army(ArmyType.ImmuneSystem, immuneSystem, boost);
            var army2 = new Army(ArmyType.Infected, infected, 0);
            winner = Fight(army1, army2);
            if (winner?.Type == ArmyType.ImmuneSystem)
                break;
        }

        return winner.UnitsLeft.ToString();
    }

    private static Army? Fight(Army army1, Army army2)
    {
        while (army1.UnitsLeft > 0 && army2.UnitsLeft > 0)
        {
            var battles = ChooseTargets(army1, army2).Concat(ChooseTargets(army2, army1)).ToList();

            var totalLosses = 0;
            foreach (var battle in battles.OrderByDescending(t => t.Attacker.Template.Initiative))
            {
                var dmg = CalculateDamage(battle.Attacker, battle.Defender);
                var losses = Math.Min(battle.Defender.Units, dmg / battle.Defender.Template.Hp);
                totalLosses += losses;
                battle.Defender.Units -= losses;
            }

            if (totalLosses == 0)
                return null;
        }

        return army1.UnitsLeft > 0 ? army1 : army2;
    }

    private static IEnumerable<Target> ChooseTargets(Army attackers, Army defenders)
    {
        var chosen = new List<Group>();
        foreach (var attacker in attackers.Where(g => g.Units > 0).OrderByDescending(g => g.EffectivePower)
                     .ThenByDescending(g => g.Template.Initiative))
        {
            var choices = defenders
                .Where(defender => defender.Units > 0 && CalculateDamage(attacker, defender) > 0)
                .Except(chosen)
                .OrderByDescending(defender => CalculateDamage(attacker, defender))
                .ThenByDescending(defender => defender.EffectivePower)
                .ThenByDescending(defender => defender.Template.Initiative)
                .ToList();

            if (choices.Count == 0)
                continue;

            chosen.Add(choices.First());
            yield return new Target(attacker, choices.First());
        }
    }

    private static int CalculateDamage(Group attacker, Group defender)
    {
        if (attacker.Units == 0)
            return 0;

        if (defender.Template.Immunity.Contains(attacker.Template.AttackType))
            return 0;

        var dmg = attacker.EffectivePower;
        if (defender.Template.Weakness.Contains(attacker.Template.AttackType))
            dmg *= 2;

        return dmg;
    }

    private static (GroupTemplate[], GroupTemplate[]) ParseInput(string input)
        => input.Paragraphs().Select(ParseArmy).ToList().Transform(armies => (armies.First(), armies.Second()));

    private static GroupTemplate[] ParseArmy(string input)
        => input.Lines().Transform(lines => lines.Skip(1).Select(ParseGroup).ToArray());

    private static GroupTemplate ParseGroup(string input)
    {
        var result = input.Extract(
            @"(\d+) units each with (\d+) hit points (?:\(([^)]+)\) )?with an attack that does (\d+) ([^ ]+) damage at initiative (\d+)")
            .ToList();

        var units = result[0].As<int>();
        var hp = result[1].As<int>();
        var weakAndImmune = result[2];
        var ad = result[3].As<int>();
        var attackType = result[4];
        var initiative = result[5].As<int>();

        var (weak, immune) = ParseWeaknessesAndImmunities(weakAndImmune);
        return new GroupTemplate(units, hp, ad, initiative, attackType, weak, immune);
    }

    private static (string[] Weak, string[] Immune) ParseWeaknessesAndImmunities(string input)
    {
        var items = input.SplitBy(";");
        if (items.Length == 0)
            return ([], []);

        return items.Length switch
        {
            1 when items[0].StartsWith("weak to") => (ParseWeaknesses(items[0]), []),
            1 => ([], ParseImmunities(items[0])),
            _ => items[0].StartsWith("weak to")
                ? (ParseWeaknesses(items[0]), ParseImmunities(items[1]))
                : (ParseWeaknesses(items[1]), ParseImmunities(items[0]))
        };
    }

    private static string[] ParseWeaknesses(string input) => input["weak to".Length..].SplitBy(",").ToArray();

    private static string[] ParseImmunities(string input) => input["immune to".Length..].SplitBy(",").ToArray();

    private record Target(Group Attacker, Group Defender);

    private record GroupTemplate(
        int Units,
        int Hp,
        int AttackDamage,
        int Initiative,
        string AttackType,
        string[] Weakness,
        string[] Immunity);

    private class Army : IEnumerable<Group>
    {
        private readonly IList<Group> _groups;

        public Army(ArmyType type, GroupTemplate[] groups, int boost)
        {
            Type = type;
            _groups = [.. groups.Select(g => new Group(g, boost))];
        }

        public ArmyType Type { get; }

        public int UnitsLeft => _groups.Sum(g => g.Units);

        public IEnumerator<Group> GetEnumerator() => _groups.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private class Group(GroupTemplate template, int boost)
    {
        public GroupTemplate Template { get; } = template;

        public int Units { get; set; } = template.Units;

        public int EffectivePower => Units * (Template.AttackDamage + boost);
    }

    private enum ArmyType
    {
        ImmuneSystem,
        Infected
    }
}

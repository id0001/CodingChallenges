using CodingChallenge.Utilities.Core.Models;
using Spectre.Console;

namespace CodingChallenge.Utilities.Core
{
    public static class ChallengeOutputFormatter
    {
        public static void PrintTitleHeader(string title, string subTitle)
        {
            AnsiConsole.Write(new Rule().DoubleBorder());
            AnsiConsole.Write(new Rule($"[White]{title} {subTitle}[/]").Centered().AsciiBorder());
            AnsiConsole.Write(new Rule().DoubleBorder());
            AnsiConsole.WriteLine();
        }

        public static void PrintChallengeHeader(int challenge)
        {
            AnsiConsole.Write(new Rule($"[blue]Challenge {challenge}[/]").Centered());
            AnsiConsole.WriteLine();
        }

        public static void PrintResult(RunResult result)
        {
            var table = new Table().Expand();
            table.Title = new TableTitle($"Part {result.Part}");
            table.AddColumn(new TableColumn("Time"));
            table.AddColumn(new TableColumn("Result"));
            table.AddColumn(new TableColumn("Expected"));

            var items = new List<Text>
            {
                new(result.Duration.ToString(), new Style(GetExecutionTimeColor(result.Duration))),
                new(result.Result),
                (!string.IsNullOrEmpty(result.ExpectedResult), result.Result == result.ExpectedResult) switch
                {
                    (true, true) => new Text(result.ExpectedResult!, new Style(Color.Green)),
                    (true, false) => new Text(result.ExpectedResult!, new Style(Color.Red)),
                    _ => new Text("-")
                }
            };

            table.AddRow(new TableRow(items));

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

        public static void PrintBenchmarkResult(BenchmarkResult result)
        {
            var table = new Table().Expand();
            table.Title = new TableTitle($"Part {result.Part}");
            table.AddColumn(new TableColumn("Time (first run)"));
            table.AddColumn(new TableColumn("Benchmark (avg over 100 runs)"));
            table.AddColumn(new TableColumn("Result"));
            table.AddColumn(new TableColumn("Expected"));

            var items = new List<Text>
            {
                new(result.Duration.ToString(), new Style(GetExecutionTimeColor(result.Duration))),
                (result.Benchmark is { }) switch
                {
                    true => new Text(result.Benchmark.Average.ToString(), new Style(GetExecutionTimeColor(result.Benchmark.Average))),
                    _ => new Text("Too long for benchmark", new Style(Color.Orange1))
                },
                new(result.Result),
                (!string.IsNullOrEmpty(result.ExpectedResult), result.Result == result.ExpectedResult) switch
                {
                    (true, true) => new Text(result.ExpectedResult!, new Style(Color.Green)),
                    (true, false) => new Text(result.ExpectedResult!, new Style(Color.Red)),
                    _ => new Text("-")
                }
            };

            table.AddRow(new TableRow(items));

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

        private static Color GetExecutionTimeColor(TimeSpan time)
        {
            if (time < TimeSpan.FromMilliseconds(100))
                return Color.Green;

            if (time < TimeSpan.FromMilliseconds(1000))
                return Color.Yellow;

            if (time < TimeSpan.FromSeconds(3))
                return Color.Orange1;

            return Color.Red;
        }
    }
}

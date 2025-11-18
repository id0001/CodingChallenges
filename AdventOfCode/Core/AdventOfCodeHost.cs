using AdventOfCode.Core.Models;
using CodingChallenge.Utilities.Core;
using Spectre.Console;

namespace AdventOfCode.Core
{
    public class AdventOfCodeHost : ChallengeHostBase<Config>
    {
        private const string BaseDirectory = "AdventOfCode";

        private static readonly FileInfo ConfigFile = new(Path.Combine(Path.GetTempPath(), BaseDirectory, "Config.json"));
        private static readonly DirectoryInfo EventFolder = new(Path.Combine(Path.GetTempPath(), BaseDirectory, "Events"));
        private static HttpClient SharedClient = new();

        private readonly int _year;

        protected override string Title => "Advent of Code";

        protected override string SubTitle { get; }

        public AdventOfCodeHost(int year)
            : base(new ConfigLoader<Config>(ConfigFile), new InputProviderFactory(year, EventFolder))
        {
            _year = year;
            SubTitle = year.ToString();
        }

        protected override async Task<Config> PromptUserForConfigAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[green]Advent of Code config[/]").LeftJustified());

            string sessionToken = await PromptForSessionTokenAsync();
            string email = await PromptForEmailAddressAsync();

            AnsiConsole.Clear();
            return new Config(sessionToken, email);
        }

        protected override async Task<bool> TestConnectionAsync(Config config)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://adventofcode.com/{_year}/day/1/input");
            request.Headers.Add("Cookie", $"session={config.SessionToken}");
            request.Headers.UserAgent.ParseAdd($"CodingChallenge.Utilities/1.0 (github.com/id0001/CodingChallenges by {config.Email})");

            var response = await SharedClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        private static async Task<string> PromptForSessionTokenAsync()
        {
            var prompt = new TextPrompt<string>("Please enter your [yellow]Advent of Code[/] session token:")
                .Validate(x => !string.IsNullOrEmpty(x), "Session token cannot be empty");

            return await AnsiConsole.PromptAsync(prompt);
        }

        private static async Task<string> PromptForEmailAddressAsync()
        {
            var prompt = new TextPrompt<string>("Please enter your email address:")
                .Validate(x => !string.IsNullOrEmpty(x), "Email cannot be empty")
                .Validate(x => x.Contains("@"), "Must be a valid email address");

            return await AnsiConsole.PromptAsync(prompt);
        }
    }
}

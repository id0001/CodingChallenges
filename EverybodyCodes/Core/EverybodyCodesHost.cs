using CodingChallenge.Utilities.Core;
using EverybodyCodes.Core.Models;
using Spectre.Console;
using System.Text.Json;

namespace EverybodyCodes.Core
{
    public class EverybodyCodesHost : ChallengeHostBase<Config>
    {
        private const string BaseDirectory = "EverybodyCodes";

        private static readonly FileInfo ConfigFile = new(Path.Combine(Path.GetTempPath(), BaseDirectory, "Config.json"));
        private static readonly DirectoryInfo EventFolder = new(Path.Combine(Path.GetTempPath(), BaseDirectory, "Events"));
        private static HttpClient SharedClient = new();

        protected override string Title => "Everybody.Codes";

        protected override string SubTitle { get; }

        public EverybodyCodesHost(int eventKey) : base(new ConfigLoader<Config>(ConfigFile), new NoteProviderFactory(eventKey, EventFolder))
        {
            SubTitle = eventKey.ToString();
        }

        protected override async Task<Config> PromptUserForConfigAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[green]Everybody.Codes config[/]").LeftJustified());

            string sessionToken = await PromptForSessionTokenAsync();
            string email = await PromptForEmailAddressAsync();
            int? seed = await GetSeedAsync(sessionToken, email);

            if (!seed.HasValue)
                throw new InvalidOperationException("Unable to retrieve seed value");

            AnsiConsole.Clear();
            return new Config { SessionToken = sessionToken, Email = email, Seed = seed.Value };
        }

        protected override async Task<bool> TestConnectionAsync(Config config)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://everybody.codes/api/user/me");
            request.Headers.Add("Cookie", $"everybody-codes={config.SessionToken}");

            var response = await SharedClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        private static async Task<string> PromptForSessionTokenAsync()
        {
            var prompt = new TextPrompt<string>("Please enter your [yellow]everybody.codes[/] session token:")
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

        private static async Task<int?> GetSeedAsync(string sessionToken, string email)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://everybody.codes/api/user/me");
            request.Headers.Add("Cookie", $"everybody-codes={sessionToken}");
            request.Headers.UserAgent.ParseAdd($"CodingChallenge.Utilities/1.0 (github.com/id0001/CodingChallenges by {email})");

            var response = await SharedClient.SendAsync(request);
            using var body = await response.Content.ReadAsStreamAsync();
            var me = await JsonSerializer.DeserializeAsync<Me>(body);
            return me?.Seed;
        }
    }
}

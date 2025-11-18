using AdventOfCode.Core;
using AdventOfCode.Core.Models;
using CodingChallenge.Utilities.Core;
using CodingChallenge.Utilities.Core.Models;
using System.Reflection;

namespace AdventOfCode.Test
{
    public class TestContext
    {
        private const string BaseDirectory = "AdventOfCode";

        private static readonly FileInfo ConfigFile = new(Path.Combine(Path.GetTempPath(), BaseDirectory, "Config.json"));
        private static readonly DirectoryInfo EventFolder = new(Path.Combine(Path.GetTempPath(), BaseDirectory, "Events"));
        private static HttpClient SharedClient = new();

        private readonly ChallengeRunner _challengeRunner;

        private TestContext(ChallengeRunner challengeRunner)
        {
            _challengeRunner = challengeRunner;
        }

        public static async Task<TestContext> Create(int year, Assembly challengeAssembly)
        {
            var configLoader = new ConfigLoader<Config>(ConfigFile);
            var providerFactory = new InputProviderFactory(year, EventFolder);

            var config = await GetConfigAsync(year, configLoader);
            if (config is null)
                throw new InvalidOperationException("Invalid config file");

            var inputProvider = providerFactory.GetProvider(config);
            var challengeRunner = new ChallengeRunner(inputProvider, challengeAssembly);

            return new TestContext(challengeRunner);
        }

        public async Task<(RunResult? Part1, RunResult? Part2)> ExecuteDayAsync(int day)
        {
            var result = await _challengeRunner.RunAsync(day, false).ToArrayAsync();
            return result.Length switch
            {
                2 => (result[0], result[1]),
                1 => (result[0], null),
                _ => (null, null)
            };
        }

        private static async Task<Config?> GetConfigAsync(int year, ConfigLoader<Config> configLoader)
        {
            try
            {
                var config = await configLoader.LoadAsync();
                if (!await TestConnectionAsync(year, config))
                    return null;

                return config;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        private static async Task<bool> TestConnectionAsync(int year, Config config)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://adventofcode.com/{year}/day/1/input");
            request.Headers.Add("Cookie", $"session={config.SessionToken}");
            request.Headers.UserAgent.ParseAdd($"CodingChallenge.Utilities/1.0 (github.com/id0001/CodingChallenges by {config.Email})");

            var response = await SharedClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}

using CodingChallenge.Utilities.Exceptions;
using CodingChallenge.Utilities.Resources;
using DocoptNet;
using Spectre.Console;

namespace CodingChallenge.Utilities.Core
{
    public abstract class ChallengeHostBase<TConfig>
    {
        private const string DocoptResourceName = "CodingChallenge.Utilities.Resources.docopt.txt";

        private readonly ConfigLoader<TConfig> _configLoader;
        private readonly IInputProviderFactory<TConfig> _inputProviderFactory;

        protected abstract string Title { get; }

        protected abstract string SubTitle { get; }

        protected ChallengeHostBase(ConfigLoader<TConfig> configLoader, IInputProviderFactory<TConfig> inputProviderFactory)
        {
            _configLoader = configLoader; ;
            _inputProviderFactory = inputProviderFactory;
        }

        public async Task RunAsync(string[] args)
        {
            var config = await GetConfigAsync();

            var inputProvider = _inputProviderFactory.GetProvider(config);
            var runner = new ChallengeRunner(inputProvider);

            var usage = await ResourceHelper.ReadAsync(DocoptResourceName);
            var arguments = new Docopt().Apply(usage, args, true, "1.0", true);
            if (arguments is null)
                return;

            var challenge = arguments["--challenge"];
            if (!challenge.IsNullOrEmpty)
            {
                // Run specific challenge
                if (!challenge.IsInt)
                    throw new InvalidOperationException("<challenge> argument must be an integer");

                await RunChallengeAsync(challenge.AsInt, runner);
            }
            else
            {
                var latest = runner.GetLatestChallenge();
                await RunChallengeAsync(latest, runner);
            }
        }

        protected abstract Task<TConfig> PromptUserForConfigAsync();

        protected abstract Task<bool> TestConnectionAsync(TConfig config);

        private async Task RunChallengeAsync(int challenge, ChallengeRunner runner)
        {
            AnsiConsole.Clear();
            ChallengeOutputFormatter.PrintTitleHeader(Title, SubTitle);
            ChallengeOutputFormatter.PrintChallengeHeader(challenge);

            if(ShouldBenchmark())
            {
                await foreach (var result in runner.BenchmarkAsync(challenge))
                {
                    ChallengeOutputFormatter.PrintBenchmarkResult(result);
                }
            }
            else
            {
                await foreach (var result in runner.RunAsync(challenge, true))
                {
                    ChallengeOutputFormatter.PrintResult(result);
                }
            }
        }

        private async Task<TConfig> GetConfigAsync()
        {
            TConfig config = default!;

            try
            {
                config = await _configLoader.LoadAsync();
            }
            catch(FileNotFoundException)
            {
                config = await CreateConfigFromUserInputAsync();
            }

            if (!await TestConnectionAsync(config))
            {
                config = await CreateConfigFromUserInputAsync();
                if (!await TestConnectionAsync(config))
                    throw new InvalidConfigException();
            }

            return config;
        }

        private async Task<TConfig> CreateConfigFromUserInputAsync()
        {
            var config = await PromptUserForConfigAsync();
            await _configLoader.SaveAsync(config);
            return config;
        }

        private static bool ShouldBenchmark()
        {
#if DEBUG
            return false;
#else
            return true;
#endif
        }
    }
}

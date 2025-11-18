using AdventOfCode.Core.Models;
using CodingChallenge.Utilities.Core;

namespace AdventOfCode.Core
{
    public class InputProvider(int year, Config config, DirectoryInfo baseDirectory) : IInputProvider
    {
        private static HttpClient SharedClient = new()
        {
            BaseAddress = new Uri("http://adventofcode.com")
        };

        public async Task<string> GetForPartAsync(int day, int part)
        {
            FileInfo file = new FileInfo(Path.Combine(baseDirectory.FullName, year.ToString(), $"{day:D2}.txt"));

            var input = await LoadFromFileAsync(file);
            if (!string.IsNullOrEmpty(input))
                return input;

            input = await LoadFromWebAsync(year, day, config);
            if (string.IsNullOrEmpty(input))
                throw new InvalidOperationException("Unable to retrieve input");

            await SaveToFileAsync(file, input);
            return input;
        }

        private async Task<string?> LoadFromFileAsync(FileInfo file)
        {
            if (!file.Exists)
                return null;

            try
            {
                return await File.ReadAllTextAsync(file.FullName);
            }
            catch
            {
                return null;
            }
        }

        private async Task SaveToFileAsync(FileInfo file, string input)
        {
            if (!baseDirectory.Exists)
                baseDirectory.Create();

            if (!file.Directory!.Exists)
                file.Directory.Create();

            await File.WriteAllTextAsync(file.FullName, input);
        }

        private static async Task<string> LoadFromWebAsync(int year, int day, Config config)
        {
            var request = CreateRequest(year, day, config);
            var response = await SharedClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Unable to retrieve input");

            var content = await response.Content.ReadAsStringAsync();
            return content
                .ReplaceLineEndings()
                .Trim(Environment.NewLine.ToCharArray());
        }

        private static HttpRequestMessage CreateRequest(int year, int day, Config config)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/{year}/day/{day}/input");
            request.Headers.Add("Cookie", $"session={config.SessionToken}");
            request.Headers.UserAgent.ParseAdd($"CodingChallenge.Utilities/1.0 (github.com/id0001/CodingChallenges by {config.Email})");
            return request;
        }
    }
}

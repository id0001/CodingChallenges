using CodingChallenge.Utilities.Core;
using EverybodyCodes.Core.Models;
using System.Text.Json;

namespace EverybodyCodes.Core
{
    public class NoteProvider(int eventKey, Config config, DirectoryInfo baseDirectory) : IInputProvider
    {
        private static HttpClient SharedClient = new();

        public async Task<string> GetForPartAsync(int challenge, int part)
        {
            FileInfo file = new FileInfo(Path.Combine(baseDirectory.FullName, eventKey.ToString(), $"{challenge:D2}.txt"));

            Notes? notes = await LoadFromFileAsync(file);
            if (notes is { } && notes.HasPart(part))
                return notes.GetPart(part);

            notes = await LoadFromWebAsync(eventKey, challenge, config);
            if (notes is null)
                throw new InvalidOperationException("Unable to retrieve input");

            await SaveToFileAsync(file, notes);
            return notes.GetPart(part);
        }

        private async Task<Notes?> LoadFromFileAsync(FileInfo file)
        {
            if (!file.Exists)
                return null;

            try
            {
                using var stream = file.OpenRead();
                return await JsonSerializer.DeserializeAsync<Notes>(stream);
            }
            catch
            {
                return null;
            }
        }

        private async Task SaveToFileAsync(FileInfo file, Notes notes)
        {
            if (!baseDirectory.Exists)
                baseDirectory.Create();

            if (!file.Directory!.Exists)
                file.Directory.Create();

            using var stream = file.OpenWrite();
            await JsonSerializer.SerializeAsync(stream, notes);
        }

        private static async Task<Notes?> LoadFromWebAsync(int eventKey, int challenge, Config config)
        {
            var notes = await DownloadNotesAsync(eventKey, challenge, config);
            var keys = await GetAesKeysAsync(eventKey, challenge, config);

            if (notes is null || keys is null)
                return null;

            return notes with
            {
                Key1 = keys.Key1,
                Key2 = keys.Key2,
                Key3 = keys.Key3
            };
        }

        private static async Task<Notes?> DownloadNotesAsync(int eventKey, int challenge, Config config)
        {
            var request = CreateInputRequest(eventKey, challenge, config);
            var response = await SharedClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Unable to retrieve input");

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Notes>(stream);
        }

        private static async Task<Keys?> GetAesKeysAsync(int eventKey, int challenge, Config config)
        {
            var request = CreateAesKeyRequest(eventKey, challenge, config);
            var response = await SharedClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Unable to retrieve keys");

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Keys>(stream);
        }

        private static HttpRequestMessage CreateInputRequest(int eventKey, int quest, Config config)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://everybody-codes.b-cdn.net/assets/{eventKey}/{quest}/input/{config.Seed}.json");
            request.Headers.Add("Cookie", $"everybody-codes={config.SessionToken}");
            request.Headers.UserAgent.ParseAdd($"CodingChallenge.Utilities/1.0 (github.com/id0001/CodingChallenges by {config.Email})");
            return request;
        }

        private static HttpRequestMessage CreateAesKeyRequest(int eventKey, int quest, Config config)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://everybody.codes/api/event/{eventKey}/quest/{quest}");
            request.Headers.Add("Cookie", $"everybody-codes={config.SessionToken}");
            request.Headers.UserAgent.ParseAdd($"CodingChallenge.Utilities/1.0 (github.com/id0001/CodingChallenges by {config.Email})");
            return request;
        }
    }
}

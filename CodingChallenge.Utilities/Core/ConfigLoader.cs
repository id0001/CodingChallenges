using System.Text.Json;

namespace CodingChallenge.Utilities.Core
{
    public class ConfigLoader<TConfig>(FileInfo configFile)
    {
        public async Task<TConfig> LoadAsync()
        {
            if (!configFile.Exists)
                throw new FileNotFoundException("Config file not found");

            using var stream = configFile.OpenRead();
            var config = await JsonSerializer.DeserializeAsync<TConfig>(stream);

            if (config is null)
            {
                configFile.Delete();
                throw new FileNotFoundException("Config file not found");
            }

            return config;
        }

        public async Task SaveAsync(TConfig config)
        {
            if (configFile.Directory is { } && !configFile.Directory.Exists)
                configFile.Directory.Create();

            using var stream = configFile.OpenWrite();
            await JsonSerializer.SerializeAsync(stream, config);
        }
    }
}

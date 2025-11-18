namespace CodingChallenge.Utilities.Resources
{
    public static class ResourceHelper
    {
        public static async Task<string> ReadAsync(string resourceName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Something went wrong while reading the resource: {resourceName}");

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static string Read(string resourceName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Something went wrong while reading the resource: {resourceName}");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}

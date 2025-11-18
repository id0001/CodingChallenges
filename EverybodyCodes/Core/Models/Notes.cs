using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace EverybodyCodes.Core.Models
{
    public record Notes
    {
        [JsonPropertyName("1")]
        public required string Note1 { get; init; }

        [JsonPropertyName("2")]
        public required string Note2 { get; init; }

        [JsonPropertyName("3")]
        public required string Note3 { get; init; }

        public string? Key1 { get; init; }

        public string? Key2 { get; init; }

        public string? Key3 { get; init; }

        public string GetPart(int part) => part switch
        {
            1 => Decrypt(Note1, Key1),
            2 => Decrypt(Note2, Key2),
            3 => Decrypt(Note3, Key3),
            _ => throw new NotImplementedException()
        };

        public bool HasPart(int part) => part switch
        {
            1 => true,
            2 => Key2 is { },
            3 => Key3 is { },
            _ => throw new NotImplementedException()
        };

        private static string Decrypt(string input, string? key)
        {
            if (key is null)
                throw new InvalidOperationException("Previous part not solved yet");

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(key.Substring(0, 16));

            using var msDecrypt = new MemoryStream(Convert.FromHexString(input));
            using var csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt
                .ReadToEnd()
                .ReplaceLineEndings()
                .Trim(Environment.NewLine.ToCharArray());
        }
    }
}

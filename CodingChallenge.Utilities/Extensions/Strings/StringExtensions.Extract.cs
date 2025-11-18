using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using static System.Object;

namespace CodingChallenge.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        extension(string source)
        {
            public IEnumerable<string> Extract([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    throw new InvalidOperationException("Regex was unsuccessful");

                return match.Groups.Values.Skip(1).Select(g => g.Value);
            }

            public bool TryExtract([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, out IEnumerable<string> matches)
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                {
                    matches = Array.Empty<string>();
                    return false;
                }

                matches = match.Groups.Values.Skip(1).Select(g => g.Value);
                return true;
            }

            public T Extract<T>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern) where T : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    throw new InvalidOperationException("Regex was unsuccessful");

                return match.Groups.Values.Skip(1).Select(g => (T)Convert.ChangeType(g.Value, typeof(T))).First();
            }

            public bool TryExtract<T>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, out T value) where T : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                value = default!;

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    return false;

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 1)
                    return false;

                value = items[0].As<T>();
                return true;
            }

            public (T1 First, T2 Second) Extract<T1, T2>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
                where T1 : IConvertible
                where T2 : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    throw new InvalidOperationException("Regex was unsuccessful");

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 2)
                    throw new InvalidOperationException($"Incorrect amount of matching groups. Expected 2, got {items.Length}");

                return (items[0].As<T1>(), items[1].As<T2>());
            }

            public bool TryExtract<T1, T2>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, out T1 first, out T2 second)
                where T1 : IConvertible
                where T2 : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                first = default!;
                second = default!;

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    return false;

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 2)
                    return false;

                (first, second) = (items[0].As<T1>(), items[1].As<T2>());
                return true;
            }

            public (T1 First, T2 Second, T3 Third) Extract<T1, T2, T3>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
                where T1 : IConvertible
                where T2 : IConvertible
                where T3 : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    throw new InvalidOperationException("Regex was unsuccessful");

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 3)
                    throw new InvalidOperationException($"Incorrect amount of matching groups. Expected 3, got {items.Length}");

                return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>());
            }

            public bool TryExtract<T1, T2, T3>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, out T1 first, out T2 second, out T3 third)
                where T1 : IConvertible
                where T2 : IConvertible
                where T3 : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                first = default!;
                second = default!;
                third = default!;

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    return false;

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 3)
                    return false;

                (first, second, third) = (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>());
                return true;
            }

            public (T1 First, T2 Second, T3 Third, T4 Fourth) Extract<T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
                where T1 : IConvertible
                where T2 : IConvertible
                where T3 : IConvertible
                where T4 : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    throw new InvalidOperationException("Regex was unsuccessful");

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 4)
                    throw new InvalidOperationException($"Incorrect amount of matching groups. Expected 4, got {items.Length}");

                return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>(), items[3].As<T4>());
            }

            public bool TryExtract<T1, T2, T3, T4>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, out T1 first, out T2 second, out T3 third, out T4 fourth)
                where T1 : IConvertible
                where T2 : IConvertible
                where T3 : IConvertible
                where T4 : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                first = default!;
                second = default!;
                third = default!;
                fourth = default!;

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    return false;

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 4)
                    return false;

                (first, second, third, fourth) = (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>(), items[3].As<T4>());
                return true;
            }

            public (T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth) Extract<T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
                where T1 : IConvertible
                where T2 : IConvertible
                where T3 : IConvertible
                where T4 : IConvertible
                where T5 : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    throw new InvalidOperationException("Regex was unsuccessful");

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 5)
                    throw new InvalidOperationException($"Incorrect amount of matching groups. Expected 5, got {items.Length}");

                return (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>(), items[3].As<T4>(), items[4].As<T5>());
            }

            public bool TryExtract<T1, T2, T3, T4, T5>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, out T1 first, out T2 second, out T3 third, out T4 fourth, out T5 fifth)
                where T1 : IConvertible
                where T2 : IConvertible
                where T3 : IConvertible
                where T4 : IConvertible
                where T5 : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                first = default!;
                second = default!;
                third = default!;
                fourth = default!;
                fifth = default!;

                var match = Regex.Match(source, pattern);
                if (!match.Success)
                    return false;

                var items = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                if (items.Length != 5)
                    return false;

                (first, second, third, fourth, fifth) = (items[0].As<T1>(), items[1].As<T2>(), items[2].As<T3>(), items[3].As<T4>(), items[4].As<T5>());
                return true;
            }

            public string[][] ExtractAllValues([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var matches = Regex.Matches(source, pattern);
                if (matches.Count == 0)
                    throw new InvalidOperationException("Regex was unsuccessful");

                return matches.Select(match =>
                    match.Groups.Values.Skip(1).Where(g => g.Success).Select(g => g.Value).ToArray()).ToArray();
            }

            public T[][] ExtractAllValues<T>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
                where T : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var matches = Regex.Matches(source, pattern);
                if (matches.Count == 0)
                    throw new InvalidOperationException("Regex was unsuccessful");

                return matches.Select(match =>
                    match.Groups.Values.Skip(1).Where(g => g.Success).Select(g => g.Value.As<T>()).ToArray()).ToArray();
            }

            public ExtractionValue<string>[] ExtractAll([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var matches = Regex.Matches(source, pattern);
                if (matches.Count == 0)
                    throw new InvalidOperationException("Regex was unsuccessful");

                return matches.Select(match =>
                {
                    var m = match.Groups[0].Value;
                    var v = match.Groups.Values.Where(g => g.Success).Skip(1).Select(g => g.Value).ToArray();
                    return new ExtractionValue<string>(m, v);
                }).ToArray();
            }

            public ExtractionValue<T>[] ExtractAll<T>([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
                where T : IConvertible
            {
                ArgumentException.ThrowIfNullOrEmpty(source);
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);

                var matches = Regex.Matches(source, pattern);
                if (matches.Count == 0)
                    throw new InvalidOperationException("Regex was unsuccessful");

                return matches.Select(match =>
                {
                    var m = match.Groups[0].Value;
                    var v = match.Groups.Values.Where(g => g.Success).Skip(1).Select(g => g.Value.As<T>()).ToArray();
                    return new ExtractionValue<T>(m, v);
                }).ToArray();
            }
        }

        public record ExtractionValue<T>(string Match, T[] Values);
    }
}

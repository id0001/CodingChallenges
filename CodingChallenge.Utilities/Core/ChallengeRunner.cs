using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Core.Models;
using System.Diagnostics;
using System.Reflection;
using TextCopy;

namespace CodingChallenge.Utilities.Core
{
    public class ChallengeRunner
    {
        private readonly SortedList<int, Type> _types;
        private readonly IInputProvider _inputProvider;

        public ChallengeRunner(IInputProvider inputProvider, System.Reflection.Assembly? assembly = null)
        {
            _inputProvider = inputProvider;

            assembly ??= System.Reflection.Assembly.GetEntryAssembly();

            var types = GetLoadableTypes(assembly!)
                .Where(t => t.GetCustomAttribute<ChallengeAttribute>() is { })
                .ToDictionary(t => t.GetCustomAttribute<ChallengeAttribute>()!.Challenge);
            _types = new SortedList<int, Type>(types);
        }

        public int GetLatestChallenge() => _types.Last().Key;

        public async IAsyncEnumerable<BenchmarkResult> BenchmarkAsync(int challenge)
        {
            var parts = GetParts(challenge);
            var instance = CreateInstance(challenge)!;
            foreach (var (attr, method) in parts)
            {
                var input = await _inputProvider.GetForPartAsync(challenge, attr.Part);
                var (result, duration) = Measure(() => CallMethod(instance, method, input));

                var benchmark = Benchmark(() => CallMethod(instance, method, input), 100, TimeSpan.FromSeconds(120));
                yield return new BenchmarkResult(challenge, attr.Part, result, attr.Expected, duration, benchmark);
            }
        }

        public async IAsyncEnumerable<RunResult> RunAsync(int challenge, bool shouldCopyResult)
        {
            var parts = GetParts(challenge);
            var instance = CreateInstance(challenge)!;
            foreach (var (attr, method) in parts)
            {
                var input = await _inputProvider.GetForPartAsync(challenge, attr.Part);
                var (result, duration) = Measure(() => CallMethod(instance, method, input));

                if (shouldCopyResult)
                    await ClipboardService.SetTextAsync(result);

                yield return new RunResult(challenge, attr.Part, result, attr.Expected, duration);
            }
        }

        private BenchmarkTime? Benchmark(Func<string> unitOfWork, int runAmount, TimeSpan? timeout = null)
        {
            var stopwatch = new Stopwatch();
            var durations = new List<TimeSpan>();
            for (var i = 0; i < runAmount; i++)
            {
                stopwatch.Restart();
                _ = unitOfWork();
                stopwatch.Stop();
                durations.Add(stopwatch.Elapsed);

                if (timeout.HasValue && stopwatch.Elapsed > timeout / runAmount)
                    return null;
            }

            return new BenchmarkTime(durations);
        }

        private (string Result, TimeSpan Duration) Measure(Func<string> unitOfWork)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = unitOfWork();
            stopwatch.Stop();
            return (result, stopwatch.Elapsed);
        }

        private KeyValuePair<PartAttribute, MethodInfo>[] GetParts(int challenge)
        {
            var type = _types[challenge];
            return GetMethods(type).OrderBy(kv => kv.Key.Part).ToArray();
        }

        private string CallMethod(object instance, MethodInfo method, string input) => (string)method.Invoke(instance, [input])!;

        private object? CreateInstance(int challenge) => Activator.CreateInstance(_types[challenge]);

        private static IEnumerable<KeyValuePair<PartAttribute, MethodInfo>> GetMethods(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly);
            return methods
                .Where(m => m.GetCustomAttribute<PartAttribute>() != null && m.GetParameters().Count() == 1 && m.GetParameters().First().ParameterType == typeof(string))
                .Select(m => new KeyValuePair<PartAttribute, MethodInfo>(m.GetCustomAttribute<PartAttribute>()!, m));
        }

        private static IEnumerable<Type> GetLoadableTypes(System.Reflection.Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t is not null).Select(t => t!);
            }
        }
    }
}

using AdventOfCode.Core.Models;
using CodingChallenge.Utilities.Core;

namespace AdventOfCode.Core
{
    public class InputProviderFactory(int year, DirectoryInfo baseDirectory) : IInputProviderFactory<Config>
    {
        public IInputProvider GetProvider(Config config) => new InputProvider(year, config, baseDirectory);
    }
}

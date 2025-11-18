using CodingChallenge.Utilities.Core;
using EverybodyCodes.Core.Models;

namespace EverybodyCodes.Core
{
    public class NoteProviderFactory(int eventKey, DirectoryInfo baseDirectory) : IInputProviderFactory<Config>
    {
        public IInputProvider GetProvider(Config config) => new NoteProvider(eventKey, config, baseDirectory);
    }
}

namespace CodingChallenge.Utilities.Core
{
    public interface IInputProviderFactory<TConfig>
    {
        public IInputProvider GetProvider(TConfig config);
    }
}

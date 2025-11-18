namespace CodingChallenge.Utilities.Core
{
    public interface IInputProvider
    {
        Task<string> GetForPartAsync(int challenge, int part);
    }
}

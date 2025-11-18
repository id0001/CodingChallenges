namespace CodingChallenge.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ChallengeAttribute(int challenge) : Attribute
    {
        public int Challenge = challenge;
    }
}

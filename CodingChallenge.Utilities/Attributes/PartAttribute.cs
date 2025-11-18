namespace CodingChallenge.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class PartAttribute(int part, string? expected = null) : Attribute
    {
        public int Part => part;

        public string? Expected => expected;
    }
}

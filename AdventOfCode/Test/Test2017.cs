
namespace AdventOfCode.Test
{
    public class Test2017 : TestBasePre2025
    {
        protected override async Task<TestContext> CreateContext() => await TestContext.Create(2017, typeof(AdventOfCode2017.Challenges.Challenge01).Assembly);
    }
}

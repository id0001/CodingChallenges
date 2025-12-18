
namespace AdventOfCode.Test
{
    public class Test2025 : TestBase
    {
        protected override async Task<TestContext> CreateContext() => await TestContext.Create(2025, typeof(AdventOfCode2025.Challenges.Challenge01).Assembly);
    }
}

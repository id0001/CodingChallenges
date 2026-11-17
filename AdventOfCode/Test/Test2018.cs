
namespace AdventOfCode.Test
{
    public class Test2018 : TestBasePre2025
    {
        protected override async Task<TestContext> CreateContext() => await TestContext.Create(2018, typeof(AdventOfCode2018.Challenges.Challenge01).Assembly);
    }
}


namespace AdventOfCode.Test
{
    public class Test2019 : TestBase
    {
        protected override async Task<TestContext> CreateContext() => await TestContext.Create(2019, typeof(AdventOfCode2019.Challenges.Challenge01).Assembly);
    }
}

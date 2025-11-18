
namespace AdventOfCode.Test
{
    public class Test2016 : TestBase
    {
        protected override async Task<TestContext> CreateContext() => await TestContext.Create(2016, typeof(AdventOfCode2016.Challenges.Challenge01).Assembly);
    }
}

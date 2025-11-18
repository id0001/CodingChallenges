namespace AdventOfCode.Test
{
    public class Test2015 : TestBase
    {
        protected override async Task<TestContext> CreateContext() => await TestContext.Create(2015, typeof(AdventOfCode2015.Challenges.Challenge01).Assembly);
    }
}

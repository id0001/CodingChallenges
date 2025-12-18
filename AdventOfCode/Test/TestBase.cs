namespace AdventOfCode.Test
{
    public abstract class TestBase : IAsyncLifetime
    {
        protected TestContext Context { get; set; } = null!;

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            Context = await CreateContext();
        }

        [SkippableTheory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(18)]
        [InlineData(19)]
        [InlineData(20)]
        [InlineData(21)]
        [InlineData(22)]
        [InlineData(23)]
        [InlineData(24)]
        [InlineData(25)]
        public async Task TestDay(int day)
        {
            var (part1, part2) = await Context.ExecuteDayAsync(day);

            Skip.If(part1 is null &&  part2 is null);

            if (part1 is { })
                Assert.Equal(part1.ExpectedResult, part1.Result);

            if (part2 is { })
                Assert.Equal(part2.ExpectedResult, part2.Result);
        }

        protected abstract Task<TestContext> CreateContext();
    }
}

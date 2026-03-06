using Soenneker.Deduplication.SlidingWindow.Registry.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Deduplication.SlidingWindow.Registry.Tests;

[Collection("Collection")]
public sealed class SlidingWindowDedupeRegistryTests : FixturedUnitTest
{
    private readonly ISlidingWindowDedupeRegistry _util;

    public SlidingWindowDedupeRegistryTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<ISlidingWindowDedupeRegistry>(true);
    }

    [Fact]
    public void Default()
    {

    }
}

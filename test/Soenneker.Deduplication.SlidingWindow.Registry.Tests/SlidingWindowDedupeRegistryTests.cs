using Soenneker.Deduplication.SlidingWindow.Registry.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Deduplication.SlidingWindow.Registry.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class SlidingWindowDedupeRegistryTests : HostedUnitTest
{
    private readonly ISlidingWindowDedupeRegistry _util;

    public SlidingWindowDedupeRegistryTests(Host host) : base(host)
    {
        _util = Resolve<ISlidingWindowDedupeRegistry>(true);
    }

    [Test]
    public void Default()
    {

    }
}

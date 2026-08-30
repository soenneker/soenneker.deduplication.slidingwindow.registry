using System;
using System.Threading.Tasks;
using AwesomeAssertions;
using Soenneker.Deduplication.SlidingWindow.Abstract;
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
    public async Task Remove_discards_history_and_configuration()
    {
        ISlidingWindowDedupe first = await _util.Get("scope-remove", TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10));
        first.TryMarkSeen("item-1").Should().BeTrue();

        (await _util.Remove("scope-remove")).Should().BeTrue();
        _util.TryGet("scope-remove", out _).Should().BeFalse();

        ISlidingWindowDedupe replacement = await _util.Get("scope-remove", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(5));
        replacement.Should().NotBeSameAs(first);
        replacement.TryMarkSeen("item-1").Should().BeTrue();
    }
}

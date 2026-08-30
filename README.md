[![](https://img.shields.io/nuget/v/soenneker.deduplication.slidingwindow.registry.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.deduplication.slidingwindow.registry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.deduplication.slidingwindow.registry/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.deduplication.slidingwindow.registry/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.deduplication.slidingwindow.registry.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.deduplication.slidingwindow.registry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.deduplication.slidingwindow.registry/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.deduplication.slidingwindow.registry/actions/workflows/codeql.yml)

# Soenneker.Deduplication.SlidingWindow.Registry

A thread-safe registry that creates and reuses one in-memory sliding-window deduplicator per string key.

## Installation

```bash
dotnet add package Soenneker.Deduplication.SlidingWindow.Registry
```

## Registration

```csharp
using Soenneker.Deduplication.SlidingWindow.Registry.Registrars;

services.AddSlidingWindowDedupeRegistryAsSingleton();
```

Singleton registration shares each key’s recent-history window across dependency-injection scopes. Use `AddSlidingWindowDedupeRegistryAsScoped()` only when each scope should have independent history.

## Usage

```csharp
using Soenneker.Deduplication.SlidingWindow.Abstract;
using Soenneker.Deduplication.SlidingWindow.Registry.Abstract;

public sealed class EventConsumer(ISlidingWindowDedupeRegistry registry)
{
    public async ValueTask<bool> ShouldProcess(string tenantId, string eventId, CancellationToken cancellationToken)
    {
        ISlidingWindowDedupe dedupe = await registry.Get(
            key: $"tenant:{tenantId}",
            window: TimeSpan.FromMinutes(10),
            rotationInterval: TimeSpan.FromSeconds(10),
            cancellationToken);

        return dedupe.TryMarkSeen(eventId);
    }
}
```

The first successful lookup creates the instance. Later calls for the same key return it even when they supply a different window or rotation interval, so keep those settings consistent.

## Releasing inactive keys

Each registry key owns a set and a background rotation timer. The number of registry keys is not bounded; do not derive keys from uncontrolled high-cardinality input.

```csharp
await registry.Remove($"tenant:{tenantId}", cancellationToken);

// Dispose every cached set and clear all history:
await registry.Clear(cancellationToken);
```

Removal disposes the cached instance. Do not continue using an instance after its key is removed. A later `Get` creates a fresh window with the newly supplied configuration. `TryGet` checks the cache without creating anything.

Dispose the registry itself at the end of its lifetime so all remaining rotation tasks are stopped.

[![](https://img.shields.io/nuget/v/soenneker.deduplication.slidingwindow.registry.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.deduplication.slidingwindow.registry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.deduplication.slidingwindow.registry/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.deduplication.slidingwindow.registry/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.deduplication.slidingwindow.registry.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.deduplication.slidingwindow.registry/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.deduplication.slidingwindow.registry/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.deduplication.slidingwindow.registry/actions/workflows/codeql.yml)

# Soenneker.Deduplication.SlidingWindow.Registry

A keyed registry of sliding window dedupe instances backed by `Soenneker.Dictionaries.Singletons.SingletonDictionary{TValue,T1,T2}`.

## Install

```bash
dotnet add package Soenneker.Deduplication.SlidingWindow.Registry
```

## Quick start

```csharp
using Soenneker.Deduplication.SlidingWindow.Registry.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddSlidingWindowDedupeRegistryAsSingleton();
```

Adds `ISlidingWindowDedupeRegistry` as a singleton service.

## What you get

- `ISlidingWindowDedupeRegistry` — A keyed registry of sliding window dedupe instances backed by `Soenneker.Dictionaries.Singletons.SingletonDictionary{TValue,T1,T2}`.
- `SlidingWindowDedupeRegistryRegistrar` — A keyed registry of sliding window dedupe instances.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `ISlidingWindowDedupeRegistry.Get(key, window, rotationInterval, cancellationToken)` | Gets the sliding window dedupe for `key`, creating and caching it with `window` and `rotationInterval` if missing. | The cached or newly created `ISlidingWindowDedupe`. |
| `ISlidingWindowDedupeRegistry.GetSync(key, window, rotationInterval, cancellationToken)` | Synchronously gets the sliding window dedupe for `key`, creating and caching it with `window` and `rotationInterval` if missing. | The resulting sliding Window Dedupe. |
| `ISlidingWindowDedupeRegistry.TryGet(key, value)` | Attempts to get a cached sliding window dedupe for `key` without creating one. | true if the requested update was applied; otherwise, false. |
| `SlidingWindowDedupeRegistryRegistrar.AddSlidingWindowDedupeRegistryAsSingleton(services)` | Adds `ISlidingWindowDedupeRegistry` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `SlidingWindowDedupeRegistryRegistrar.AddSlidingWindowDedupeRegistryAsScoped(services)` | Adds `ISlidingWindowDedupeRegistry` as a scoped service. | The same service collection, so additional registrations can be chained. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
- Calls that return a cached or singleton value reuse the same instance until the owning service is disposed.
- Dispose instances you own when their scope ends so held resources can be released.

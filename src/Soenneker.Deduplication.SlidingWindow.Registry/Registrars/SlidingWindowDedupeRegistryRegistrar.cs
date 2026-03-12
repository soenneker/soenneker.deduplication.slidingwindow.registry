using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Deduplication.SlidingWindow.Registry.Abstract;

namespace Soenneker.Deduplication.SlidingWindow.Registry.Registrars;

/// <summary>
/// A keyed registry of sliding window dedupe instances.
/// </summary>
public static class SlidingWindowDedupeRegistryRegistrar
{
    /// <summary>
    /// Adds <see cref="ISlidingWindowDedupeRegistry"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddSlidingWindowDedupeRegistryAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<ISlidingWindowDedupeRegistry, SlidingWindowDedupeRegistry>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ISlidingWindowDedupeRegistry"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddSlidingWindowDedupeRegistryAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<ISlidingWindowDedupeRegistry, SlidingWindowDedupeRegistry>();

        return services;
    }
}

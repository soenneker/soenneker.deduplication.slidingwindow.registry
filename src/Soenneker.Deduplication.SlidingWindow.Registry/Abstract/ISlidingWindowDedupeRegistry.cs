using System;
using System.Threading.Tasks;
using Soenneker.Deduplication.SlidingWindow.Abstract;

namespace Soenneker.Deduplication.SlidingWindow.Registry.Abstract;

/// <summary>
/// A keyed registry of sliding window dedupe instances backed by <see cref="Soenneker.Dictionaries.Singletons.SingletonDictionary{TValue,T1,T2}"/>.
/// </summary>
public interface ISlidingWindowDedupeRegistry : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the sliding window dedupe for <paramref name="key"/>, creating and caching it with
    /// <paramref name="window"/> and <paramref name="rotationInterval"/> if missing.
    /// </summary>
    /// <param name="key">Registry key (e.g. scope or stream name).</param>
    /// <param name="window">Total deduplication duration.</param>
    /// <param name="rotationInterval">How frequently expiration buckets rotate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cached or newly created <see cref="ISlidingWindowDedupe"/>.</returns>
    ValueTask<ISlidingWindowDedupe> Get(string key, TimeSpan window, TimeSpan rotationInterval, System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronously gets the sliding window dedupe for <paramref name="key"/>, creating and caching it with
    /// <paramref name="window"/> and <paramref name="rotationInterval"/> if missing.
    /// </summary>
    ISlidingWindowDedupe GetSync(string key, TimeSpan window, TimeSpan rotationInterval, System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to get a cached sliding window dedupe for <paramref name="key"/> without creating one.
    /// </summary>
    bool TryGet(string key, out ISlidingWindowDedupe? value);
}

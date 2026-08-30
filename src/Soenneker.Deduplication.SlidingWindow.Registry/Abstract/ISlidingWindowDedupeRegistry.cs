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
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="window">Window for the get sync operation.</param>
    /// <param name="rotationInterval">Interval between key or resource rotations.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The resulting sliding Window Dedupe.</returns>
    ISlidingWindowDedupe GetSync(string key, TimeSpan window, TimeSpan rotationInterval, System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to get a cached sliding window dedupe for <paramref name="key"/> without creating one.
    /// </summary>
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="value">Receives the matching value when the lookup succeeds.</param>
    /// <returns>true if the requested update was applied; otherwise, false.</returns>
    bool TryGet(string key, out ISlidingWindowDedupe? value);

    /// <summary>
    /// Removes and disposes the dedupe instance for <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Registry key to remove.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when an instance was removed; otherwise <c>false</c>.</returns>
    ValueTask<bool> Remove(string key, System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronously removes and disposes the dedupe instance for <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Registry key to remove.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when an instance was removed; otherwise <c>false</c>.</returns>
    bool RemoveSync(string key, System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes and disposes every cached dedupe instance.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    ValueTask Clear(System.Threading.CancellationToken cancellationToken = default);
}

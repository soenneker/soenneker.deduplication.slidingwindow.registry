using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Deduplication.SlidingWindow.Abstract;
using Soenneker.Deduplication.SlidingWindow.Registry.Abstract;
using Soenneker.Dictionaries.Singletons;
namespace Soenneker.Deduplication.SlidingWindow.Registry;
public sealed class SlidingWindowDedupeRegistry : ISlidingWindowDedupeRegistry
{
    private readonly SingletonDictionary<ISlidingWindowDedupe, TimeSpan, TimeSpan> _dictionary;
    /// <summary>
    /// Returns the configured sliding Window Dedupe used by the sliding window dedupe registry.
    /// </summary>
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="window">Length of time during which duplicate values are rejected.</param>
    /// <param name="rotationInterval">How often the registry rotates its internal window buckets.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested sliding Window Dedupe.</returns>
    public SlidingWindowDedupeRegistry()
    {
        _dictionary = new SingletonDictionary<ISlidingWindowDedupe, TimeSpan, TimeSpan>();
        _dictionary.SetInitialization(static (_, window, rotationInterval) => new SlidingWindowXxHashDedupe(window, rotationInterval));
    }
    /// <summary>
    /// Returns the configured sliding Window Dedupe used by the sliding window dedupe registry.
    /// </summary>
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="window">Window for the get operation.</param>
    /// <param name="rotationInterval">Interval between key or resource rotations.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The requested sliding Window Dedupe.</returns>
    public ValueTask<ISlidingWindowDedupe> Get(string key, TimeSpan window, TimeSpan rotationInterval, CancellationToken cancellationToken = default) =>
        _dictionary.Get(key, window, rotationInterval, cancellationToken);

    /// <summary>
    /// Returns the configured sliding Window Dedupe used by the Sliding Window Dedupe Registry.
    /// </summary>
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="window">Window for the get sync operation.</param>
    /// <param name="rotationInterval">Interval between key or resource rotations.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The requested sliding Window Dedupe.</returns>
    public ISlidingWindowDedupe GetSync(string key, TimeSpan window, TimeSpan rotationInterval, CancellationToken cancellationToken = default) =>
        _dictionary.GetSync(key, window, rotationInterval, cancellationToken);

    public bool TryGet(string key, out ISlidingWindowDedupe? value) =>
        _dictionary.TryGet(key, out value);

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose() => _dictionary.Dispose();

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask DisposeAsync() => _dictionary.DisposeAsync();
}

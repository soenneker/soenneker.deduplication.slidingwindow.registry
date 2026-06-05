using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Deduplication.SlidingWindow.Abstract;
using Soenneker.Deduplication.SlidingWindow.Registry.Abstract;
using Soenneker.Dictionaries.Singletons;

namespace Soenneker.Deduplication.SlidingWindow.Registry;

/// <inheritdoc cref="ISlidingWindowDedupeRegistry"/>
public sealed class SlidingWindowDedupeRegistry : ISlidingWindowDedupeRegistry
{
    private readonly SingletonDictionary<ISlidingWindowDedupe, TimeSpan, TimeSpan> _dictionary;

    public SlidingWindowDedupeRegistry()
    {
        _dictionary = new SingletonDictionary<ISlidingWindowDedupe, TimeSpan, TimeSpan>();
        _dictionary.SetInitialization(static (_, window, rotationInterval) => new SlidingWindowXxHashDedupe(window, rotationInterval));
    }

    public ValueTask<ISlidingWindowDedupe> Get(string key, TimeSpan window, TimeSpan rotationInterval, CancellationToken cancellationToken = default) =>
        _dictionary.Get(key, window, rotationInterval, cancellationToken);

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
using System.Collections.Concurrent;

namespace Scopos.BabelFish.Helpers {

    /// <summary>
    /// Generic Cache instance.
    /// <para>This is thread safe.</para>
    /// </summary>
    /// <remarks>This code was patially written by AI.</remarks>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Cache<TKey, TValue> : IClearCache {
        private readonly TimeSpan _expiration;
        private readonly ConcurrentDictionary<TKey, CacheEntry> _store = new();

        public Cache() {
            _expiration = TimeSpan.FromSeconds( 60 );
        }

        public Cache( TimeSpan expiration ) {
            _expiration = expiration;
        }

        public void AddValue( TKey key, TValue value ) {
            _store[key] = new CacheEntry( value, DateTime.UtcNow );
        }

        public bool TryGetValue( TKey key, out TValue value ) {
            if (_store.TryGetValue( key, out var entry )) {
                if (DateTime.UtcNow - entry.Timestamp <= _expiration) {
                    value = entry.Value;
                    return true;
                }
            }

            value = default!;
            return false;
        }

        /// <summary>
        /// Removes the passed in key and its value from the cache.
        /// </summary>
        /// <param name="key"></param>
        public void RemoveValue( TKey key ) {
            _store.TryRemove( key, out _ );
        }

        /// <inheritdoc />
        public void ClearCache() {
            _store.Clear();
        }

        private class CacheEntry {
            public TValue Value { get; }
            public DateTime Timestamp { get; }

            public CacheEntry( TValue value, DateTime timestamp ) {
                Value = value;
                Timestamp = timestamp;
            }
        }

        public int Count {
            get {
                return _store.Count;
            }
        }
    }
}

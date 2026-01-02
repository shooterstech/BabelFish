using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Helpers {

    /// <summary>
    /// Generic Cache instance.
    /// <para>This is thread safe.</para>
    /// </summary>
    /// <remarks>This code was patially written by AI.</remarks>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Cache<TKey, TValue> {
        private readonly TimeSpan _expiration;
        private readonly ConcurrentDictionary<TKey, CacheEntry> _store = new();

        public Cache( ) {
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

        private class CacheEntry {
            public TValue Value { get; }
            public DateTime Timestamp { get; }

            public CacheEntry( TValue value, DateTime timestamp ) {
                Value = value;
                Timestamp = timestamp;
            }
        }
    }
}

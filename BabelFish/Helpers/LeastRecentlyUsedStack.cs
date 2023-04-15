using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime;

namespace BabelFish.Helpers {

    /// <summary>
    /// Implements a LeastRecnetlyUsedStatck with generic type T.
    /// Class is written to be thread safe.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LeastRecentlyUsedStack<T> {

        //NOTE: this is not a true stack, as we can move objects in the stack to the top when they are seen.
        private List<T> stack = new List<T>();
        private Dictionary<T, DateTime> lastSeen = new Dictionary<T, DateTime>();
        private object mutex = new object();
        private int maxSize = 100;

        /// <summary>
        /// Called when an item is removed from the cache
        /// </summary>
        public EventHandler<EventArgs<T>> OnRemovedFromCache;

        /// <summary>
        /// Marks the item as seen. Making it the most recently seen item. 
        /// </summary>
        /// <param name="item"></param>
        public void Seen( T item ) {
            lock (mutex) {
                stack.Remove( item );

                stack.Insert( 0, item );
                lastSeen[item] = DateTime.UtcNow;
                CleanUp();
            }
        }

        /// <summary>
        /// The maximum number of items this LeastRecentlyUsedStack will store before removing the oldest items.
        /// </summary>
        public int MaxSize { 
            get { return maxSize; }
            set {
                if (value < 1)
                    throw new ArgumentOutOfRangeException( "MaxSize must be at least 1 ... else you really don't have much of a cache." );
                maxSize = value;
                CleanUp();
            }
        }

        /// <summary>
        /// Returns the DateTime when the passed in item was last seen, in UTC format.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public DateTime LastSeen( T item ) {
            DateTime dateTime;
            lock (mutex) {
                if (lastSeen.TryGetValue( item, out dateTime )) {
                    return dateTime;
                }
            }

            throw new KeyNotFoundException($"The item '{item}' is not currently stored in the cache.");
        }

        /// <summary>
        /// Removed items from the statck, if the length of the stack exceeds MaxSize.
        /// Invokes OnRemoveFromCache if an item is removed.
        /// </summary>
        private void CleanUp() {

            List<T> itemsRemoved = new List<T>();

            lock (mutex) {
                while (stack.Count > MaxSize) {
                    //Remove the last item.
                    T item = stack[stack.Count - 1];
                    stack.RemoveAt( stack.Count - 1 );
                    itemsRemoved.Add( item );
                }
            }

            if ( OnRemovedFromCache != null ) {
                foreach( T item in itemsRemoved ) {
                    OnRemovedFromCache.Invoke( this, new EventArgs<T>(item) );
                }
            }
        }
    }
}

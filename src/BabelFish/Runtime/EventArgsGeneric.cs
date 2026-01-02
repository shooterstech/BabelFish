using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Runtime {

    /// <summary>
    /// Generic version of EventArgs that can take on any class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArgs<T> : EventArgs {

        public T Value { get; set; }

        public EventArgs() { }

        public EventArgs( T value ) {
            this.Value = value;
        }
    }
}

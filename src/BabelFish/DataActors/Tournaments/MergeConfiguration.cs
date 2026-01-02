using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public abstract class MergeConfiguration {

        /// <summary>
        /// Concrete class identifier. Its value will be the same value as the cooresponding
        /// MergeMethod class' .Method.
        /// </summary>
        public string Method { get; protected set; }
    }
}

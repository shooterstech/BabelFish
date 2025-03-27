using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// The Children of the Event are listed explicitly. 
    /// </summary>
    public class EventExplicit : Event {

        public EventExplicit() {
            Derivation = EventDerivationType.EXPLICIT;
        }

        /// <inheritdoc />
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public override List<string> Children { 
            get {
                return _children;
            }
            set { 
                _children = value; 
            }
        }
    }
}

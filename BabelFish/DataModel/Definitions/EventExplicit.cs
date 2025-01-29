using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class EventExplicit : Event {

        public EventExplicit() {
            Derivation = EventDerivationType.EXPLICIT;
        }
        
        /// <inheritdoc />
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

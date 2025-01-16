using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.AttributeValue {

    public enum VisibilityOption { 
        /// <summary>
        /// Only the owner of the data may see this value.
        /// </summary>
        PRIVATE,

        /// <summary>
        /// Only the owner of the data may see this value. However, the value may be used in the calculation of a derived attribute value that is PUBLIC or PROTECTED
        /// </summary>
        INTERNAL,

        /// <summary>
        /// Only the owner, and people in a sharing relationship with the owner (e.g. a Coach) may see thsi value.
        /// </summary>
        PROTECTED, 
        
        /// <summary>
        /// Globally readable, everyone may see this value.
        /// </summary>
        PUBLIC };
}

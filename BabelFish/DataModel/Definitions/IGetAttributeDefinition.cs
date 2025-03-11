using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Classes that reference Attribute Definitions should implement this interface.
    /// </summary>
    public interface IGetAttributeDefinition {

        /// <summary>
        /// Retreives the Attribute Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
		Task<Attribute> GetAttributeDefinitionAsync();
    }
}

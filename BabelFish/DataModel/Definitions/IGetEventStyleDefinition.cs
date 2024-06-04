using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Classes that reference EventStyle Definitions should implement this interface.
    /// </summary>

    public interface IGetEventStyleDefinition {

        /// <summary>
        /// Retreives the EventStyle Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
		Task<EventStyle> GetEventStyleDefinitionAsync();
    }
}

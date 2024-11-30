using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Classes that reference EVENT AND STAGE STYLE Definitions should implement this interface.
    /// </summary>
    interface IGetEventAndStageStyleMapping {

        /// <summary>
        /// Retreives the EVENT AND STAGE STYLE MAPPING Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
		Task<EventAndStageStyleMapping> GetEventAndStageStyleMappingDefinitionAsync();
    }
}

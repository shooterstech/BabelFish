using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Classes that reference EVENT STYLE Definitions should implement this interface.
    /// </summary>

    public interface IGetEventStyleDefinition {

        /// <summary>
        /// Retreives the EVENT STYLE Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
		Task<EventStyle> GetEventStyleDefinitionAsync();
    }


    /// <summary>
    /// Classes that reference multiple EVENT STYLE Definitions should implement this interface.
    /// </summary>
    public interface IGetEventStyleDefinitionList {

        /// <summary>
        /// Retreives the EVENT STYLE Definitions referenced by the instantiating class.
        /// Key is the set name, value is the definition.
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, EventStyle>> GetEventStyleDefinitionListAsync();
    }
}

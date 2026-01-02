using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Classes that reference Target Definitions should implement this interface.
    /// </summary>
    public interface IGetTargetDefinition {

        /// <summary>
        /// Retreives the Target Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
		Task<Target> GetTargetDefinitionAsync();
    }

    /// <summary>
    /// Classes that reference a list of Target Definitions should implement this interface.
    /// </summary>
    public interface IGetTargetDefinitionList {

        /// <summary>
        /// Retreives the Target Definition referenced by the instantiating class.
        /// Key is the set name, value is the definition.
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<SetName, Target>> GetTargetDefinitionListAsync();
    }
}

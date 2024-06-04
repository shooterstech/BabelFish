using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions
{

    /// <summary>
    /// Classes that reference Target Collection Definitions should implement this interface.
    /// </summary>
    public interface IGetTargetCollectionDefinition
    {

        /// <summary>
        /// Retreives the Target Collection Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
        Task<TargetCollectionDefinition> GetTargetCollectionDefinitionAsync();
    }
}

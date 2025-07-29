using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Classes that reference STAGE STYLE Definitions should implement this interface.
    /// </summary>
    public interface IGetStageStyleDefinition {

        /// <summary>
        /// Retreives the STAGE STYLE Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
        Task<StageStyle> GetStageStyleDefinitionAsync();
    }


    /// <summary>
    /// Classes that reference multiple STAGE STYLE Definitions should implement this interface.
    /// </summary>
    public interface IGetStageStyleDefinitionList {

        /// <summary>
        /// Retreives the STAGE STYLE Definitions referenced by the instantiating class.
        /// Key is the set name, value is the definition.
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, StageStyle>> GetStageStyleDefinitionListAsync();
    }
}

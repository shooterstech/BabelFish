using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Classes that reference SCORE FORMAT COLLECTION Definitions should implement this interface.
    /// </summary>
    interface IGetScoreFormatCollectionDefinition {

        /// <summary>
        /// Retreives the SCORE FORMAT COLLECTION Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
		Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync();
    }
}

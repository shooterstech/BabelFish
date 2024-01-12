using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Classes that reference ResultListFormat Definitions should implement this interface.
    /// </summary>
    public interface IGetResultListFormatDefinition {

        /// <summary>
        /// Retreives the ResultListFormat Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
        Task<ResultListFormat> GetResultListFormatDefinitionAsync();
    }
}

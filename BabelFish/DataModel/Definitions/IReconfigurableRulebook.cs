using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Standard properties that all Reconfigurable Rulebook Objects should include. 
    /// </summary>
    public interface IReconfigurableRulebookObject {

        /// <summary>
        /// Internal documentation comments. All text is ignored by the system.
        /// </summary>
        string Comment { get; set; }
    }
}

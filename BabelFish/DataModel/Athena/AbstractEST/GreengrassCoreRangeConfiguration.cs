using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Athena.Range;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST {
    
    /// <summary>
    /// Represents the data structure of an Greengrass Core (v2) named shadow for Range Configuration.
    /// </summary>
    public class GreengrassCoreRangeConfiguration {

        public RangeConfiguration RangeConfiguration { get; set; }
    }
}

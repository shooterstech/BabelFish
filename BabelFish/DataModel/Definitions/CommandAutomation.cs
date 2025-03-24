using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions
{
    public abstract class CommandAutomation
    {
        // maybe need to add action for applying penalties?
        // I am not sure why that would go here, and not immediately be applied by shot attributes.
        //      (which should in turn apply a remark.... maybe I see it...) - Liam

        /// <summary>
        /// Concret class identifier
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include)]
        public CommandAutomationSubject Subject { get; set; } = CommandAutomationSubject.NONE;
    }
}

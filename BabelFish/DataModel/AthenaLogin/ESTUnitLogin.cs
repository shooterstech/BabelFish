using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.AthenaLogin {
    public class ESTUnitLogin {

        /// <summary>
        /// The name of the EST Unit that the user is trying to log into.
        /// </summary>
        public string ThingName { get; set; }

        public ESTUnitLoginSession Session { get; set; }
    }
}

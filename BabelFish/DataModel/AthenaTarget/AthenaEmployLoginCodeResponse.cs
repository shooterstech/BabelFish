using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.AthenaTarget {
    public class AthenaEmployLoginCodeResponse : BaseClass {

        /// <summary>
        /// The name of the EST Target that the user is trying to log into.
        /// </summary>
        public string ThingName { get; set; }
    }
}

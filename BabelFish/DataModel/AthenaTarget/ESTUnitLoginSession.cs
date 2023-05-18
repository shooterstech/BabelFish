using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.AthenaTarget {
    public class ESTUnitLoginSession {

        public string SessionCode { get; set; }

        /// <summary>
        /// String formatted as a Date Time
        /// </summary>
        public string SessionExpriation { get; set; }

        public ESTUnitLoginUser User { get; set; }
    }
}

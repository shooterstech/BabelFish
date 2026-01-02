using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class MonitorState {

        public MonitorState() {

        }

        /// <summary>
        /// The FiringLineLabel that cooresponds to the FiringLine that this monitor is currently occupying.
        /// </summary>
        public string CurrentFiringLine { get; set; }

        /// <summary>
        /// Boolean indicating if this Monitor is currently avaliable for use.
        /// </summary>
        public bool Active { get; set; }
    }
}

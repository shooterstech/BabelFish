using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.Range {
    public class Monitor {

        public Monitor() {

        }

        /// <summary>
        /// The network address (IoT thing name) of Monitor
        /// </summary>
        public string MonitorStateAddress { get; set; }

        /// <summary>
        /// The FiringLineLables that this Monitor can be a part of. In most cases exactly one FiringLineLabel will be listed.
        /// </summary>
        public List<string> MonitorLineLabels { get; set; }

        /// <summary>
        /// Human readable name for this Monitor.
        /// </summary>
        public string Nickname{ get; set; }
    }
}

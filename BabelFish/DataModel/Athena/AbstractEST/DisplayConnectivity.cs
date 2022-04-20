using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST {
    public class DisplayConnectivity {

        public DisplayConnectivity() {
            AssociatedMonitors = new Dictionary<string, AssociatedMonitor>();

            DisassociatedMonitors = new List<string>();
        }

        /// <summary>
        /// Monitors are responsible for setting their values within AssociatedMonitors
        /// A Monitor is connected to a display through MonitorState["AssociatedDisplays"]
        /// the Key to the AssociatedMonitors dictionary is the firing point number.
        /// </summary>
        public Dictionary<string, AssociatedMonitor> AssociatedMonitors { get; set; }

        /// <summary>
        /// To remove a monitor from teh AssociatedMonitors list, add the IoT Name of the monitor to DisassociatedMonitors
        /// </summary>
        public List<string> DisassociatedMonitors { get; set; }
    }

    public class AssociatedMonitor {

        public string MatchID { get; set; }

        public string RCOFID { get; set; }

        public string TargetDefinition { get; set; }
    }
}

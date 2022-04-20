using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;

namespace ShootersTech.DataModel.Athena.Ping {
    public class Heartbeat {

        public Heartbeat() {
            ESTStatusList = new Dictionary<string, ESTStatus>();
        }

        public int ShotCount { get; set; }

        public string Owner { get; set; }

        public string GGG { get; set; }

        /// <summary>
        /// Key is the thing name
        /// Value is a summary of the EST Unit's state.
        /// </summary>
        public Dictionary<string, ESTStatus> ESTStatusList { get; set; }
    }
}

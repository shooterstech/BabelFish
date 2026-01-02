using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Scopos.BabelFish.DataModel.Athena.ESTUnitCommands {

    /// <summary>
    /// Helper class to log the commands / messages sent to EST Units.
    /// </summary>
    public class ESTUnitCommandMessage {

        /// <summary>
        /// The complete payload of the message / command that was sent.
        /// </summary>
        public ESTUnitCommandPayload Payload { get; set; }

        //The time the command / message was sent
        public DateTime TimeSent { get; set; }

        //Thing name of the EST Unit the message was sent to
        public string ESTUnitName { get; set; }

        public override string ToString() {
            return $"{Payload.Command} command sent to {ESTUnitName} at {TimeSent}";
        }
    }
}

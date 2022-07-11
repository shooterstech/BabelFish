using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {
    
    /// <summary>
    /// Payload object of a command to send to an EST Unit
    /// </summary>
    public class ESTUnitCommandPayload {

        public ESTUnitCommandPayload() {
            MessageID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The command to send to the EST Unit
        /// </summary>
        public ESTUnitCommands Command { get; set; }

        /// <summary>
        /// The parameters to send with the command. NOTE, not all commands have parameters.
        /// The key is the ItemID. Which is controlled by the sender. If not is specified by the 
        /// caller a value of "_" is used.
        /// </summary>
        public Dictionary<string, CommandParameters> Items { get; set; }

        /// <summary>
        /// Every Command has an optinoal MessageID field. While not required, by providing a 
        /// unique value, any error or success message can be mapped back to this command with this 
        /// message ID.
        /// Optional unique message ID
        /// </summary>
        public string MessageID { get; set; }
    }
}

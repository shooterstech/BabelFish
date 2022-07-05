using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {

    /// <summary>
    /// Class representing the data structure of a response to a command to an EST Unit
    /// </summary>
    public class CommandResponsePayload {

        /// <summary>
        /// Public constructor
        /// </summary>
        public CommandResponsePayload() { }

        /// <summary>
        /// The original command that was issued.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ESTUnitCommands Command { get; set; }

        /// <summary>
        /// The original message id that was sent. 
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// Item respnoses from each command request item.
        /// </summary>
        public Dictionary<string, ItemResponse> ItemResponses { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {
    internal class ParametersRequestShots : CommandParameters {

        public const int CONCRETE_CLASS_ID = 8;

        public ParametersRequestShots() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// The MatchID (formatted as a match id), the requestor wants to have all the shots for.
        /// This field is required.
        /// </summary>
        public string MatchID { get; set; }

        /// <summary>
        /// The ResultCOFID (formatted as a UUID), the requestor wants to have all the shots for.
        /// This field is not required.
        /// </summary>
        public string ResultCOFID { get; set; } = "";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {

    /// <summary>
    /// Parameters for sending a request for a Result COF data packet
    /// </summary>
    public class ParametersRequestResultCOF : CommandParameters {

        public const int CONCRETE_CLASS_ID = 7;

        public ParametersRequestResultCOF() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// Parameter to request if all shots are returned.
        /// If true, all shots are returned. If false, only the last three shots are returned.
        /// Default value is false.
        /// </summary>
        public bool AllShots { get; set; } = false;
    }
}

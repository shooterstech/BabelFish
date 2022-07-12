using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {
    public class ParametersUpdateShots : CommandParameters {

        public const int CONCRETE_CLASS_ID = 9;

        public ParametersUpdateShots() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <inheritdoc/>
        public int ConcreteClassId { get; set; }

        /// <summary>
        /// The Shot to update
        /// </summary>
        public ShootersTech.BabelFish.DataModel.Athena.Shot.Shot Shot { get; set; }
    }
}

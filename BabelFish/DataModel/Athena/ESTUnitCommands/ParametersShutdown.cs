using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {

    /// <summary>
    /// Parameters for a Shutdown command.
    /// 
    /// NOTE: This command does not take any special parameters.
    /// </summary>
    public class ParametersShutdown : CommandParameters {

        public const int CONCRETE_CLASS_ID = 3;

        public ParametersShutdown() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {

    /// <summary>
    /// Parameters for a hard reboot command.
    /// 
    /// NOTE: This command does not take any specific parameters
    /// </summary>
    public class ParametersHardReboot : CommandParameters {

        public const int CONCRETE_CLASS_ID = 2;

        public ParametersHardReboot() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }
    }
}

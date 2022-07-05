using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {


    /// <summary>
    /// Parameters for a Soft Reboot command.
    /// 
    /// The Soft Reboot command does not take any special parameters
    /// </summary>
    public class ParametersSoftReboot : CommandParameters {

        public const int CONCRETE_CLASS_ID = 1;

        public ParametersSoftReboot() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.ESTUnitCommands {

    /// <summary>
    /// Parameters for SaveStageFile command.
    /// 
    /// NOTE: This command does not take any specific parameters
    /// </summary>
    public class ParametersSaveStateFile : CommandParameters {

        public const int CONCRETE_CLASS_ID = 10;

        public ParametersSaveStateFile() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }
    }
}

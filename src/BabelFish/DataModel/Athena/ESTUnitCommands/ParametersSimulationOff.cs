using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.ESTUnitCommands {
    public class ParametersSimulationOff : CommandParameters {

        public const int CONCRETE_CLASS_ID = 4;

        public ParametersSimulationOff() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }
    }
}

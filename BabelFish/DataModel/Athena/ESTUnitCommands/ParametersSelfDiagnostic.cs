using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.ESTUnitCommands {
    public class ParametersSelfDiagnostic : CommandParameters {

        public const int CONCRETE_CLASS_ID = 5;

        public ParametersSelfDiagnostic() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }
    }
}

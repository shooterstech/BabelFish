using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.ESTUnitCommands {
    public class ParametersRotateDisplay : CommandParameters {

        public const int CONCRETE_CLASS_ID = 6;

        public ParametersRotateDisplay() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }
    }
}

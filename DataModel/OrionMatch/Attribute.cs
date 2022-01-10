using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Match {

    /// <summary>
    /// This class represents an Orion match attribute pre-Reconfigurable Rulebook. In time this class will be
    /// replaced with DataModel.Match.Attribute, which is the Reconfigurable Rulebook version.
    /// </summary>
    [Serializable]
    public class Attribute {

        public Attribute() {
            Values = new List<string>();
        }

        public string AttributeDef { get; set; }

        public List<string> Values { get; set; }
    }
}

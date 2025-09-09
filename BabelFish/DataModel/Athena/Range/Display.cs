using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class Display {

        public Display() {

        }
        /// <summary>
        /// A unique orion ID and thing name of the device.
        /// </summary>
        public string DisplayStateAddress {get; set;}

        /// <summary>
        /// A human readable name given to this display group.
        /// Traditionally in the form of "Display 1"
        /// </summary>
        public string Nickname { get; set; }

    }
}

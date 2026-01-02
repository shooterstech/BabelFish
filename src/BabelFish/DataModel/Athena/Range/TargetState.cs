using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class TargetState {

        public TargetState() {

        }

        /// <summary>
        /// The TargetLineLabel that cooresponds to the TargetLine that this target is currently occupying.
        /// </summary>
        public string CurrentTargetLine { get; set; }

        /// <summary>
        /// Boolean indicating if this Target is currently avaliable for use.
        /// </summary>
        public bool Active { get; set; }
    }
}

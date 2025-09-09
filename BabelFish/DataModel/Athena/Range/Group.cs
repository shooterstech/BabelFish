using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class Group {

        public Group() {

        }

        /// <summary>
        /// A human readable name given to this Target Group.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// A unique, within the orion license account, id for this Target Group. Usually a short semi-readable name.
        /// </summary>
        public string GroupLabel { get; set; }

        /// <summary>
        /// A human readable description of the Target Group.
        /// </summary>
        public string Description { get; set; }
    }
}

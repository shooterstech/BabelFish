using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST {

    /// <summary>
    /// Class represenging the assinged firing point and EST Target for an EST Display.
    /// </summary>
    public class AssignedTarget {

        public AssignedTarget() { }

        /// <summary>
        /// The assigned firing point.
        /// </summary>
        public string FiringPoint { get; set; }

        /// <summary>
        /// The Target's IoT Thing Name
        /// </summary>
        public string TargetName { get; set; }

        public override string ToString() {
            return $"{FiringPoint}";
        }
    }
}

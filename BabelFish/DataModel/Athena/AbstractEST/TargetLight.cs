using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST {
    public class TargetLight {

        public TargetLight() {

        }

        public bool Capability { get; set; }

        public int DefaultIllumination { get; set; }

        public int EstimatedLux { get; set; }

        /// <summary>
        /// On a scale of 0 to 100, the amount of light the target is currently illuminating.
        /// </summary>
        public int Illumination { get; set; }
    }
}

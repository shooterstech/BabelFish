using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class FiringLine {

        public FiringLine() {

        }

        /// <summary>
        /// A human readable name given to this firing line.
        /// </summary>
        public string FiringLineName { get; set; }

        /// <summary>
        /// A unique, within the orion license account, id for this firing line. Usually a short semi-readable name.
        /// </summary>
        public string FiringLineLabel { get; set; }

        /// <summary>
        /// A human readable description of the Firing LIne.
        /// </summary>
        public string Description { get; set; }

        public static FiringLine DefaultFiringLine {
            get {
                var fl = new FiringLine() {
                    FiringLineLabel = "10m",
                    FiringLineName = "10m Air Gun",
                    Description = "10m air gun firing line."
                };
                return fl;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class FiringPoint {

        public FiringPoint() {

            Group = "";
        }

        public string FiringPointNumber { get; set; }

        public string Group { get; set; }

        public List<Target> Targets { get; set; }

        public List<Monitor> Monitors { get; set; }
    }
}

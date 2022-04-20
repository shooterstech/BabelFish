using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST {
    public class LiveView {

        public bool Capability { get; set; }

        public string Directory { get; set; }

        public bool Enabled { get; set; }

        public string Error { get; set; }

        public string Event { get; set; }

        public int Frequency { get; set; }

        public string LastPhoto { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.OrionMatch {
    [Serializable]
    public class ResultListAbbr {

        public ResultListAbbr() {

        }

        public string Status { get; set; }

        public string ResultName { get; set; }

        public bool DefaultScoreboard { get; set; }

        public bool Team { get; set; }

        public bool Primary { get; set; }

    }
}

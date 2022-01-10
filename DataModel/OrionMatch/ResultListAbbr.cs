using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Match {
    [Serializable]
    public class ResultListAbbr {

        public ResultListAbbr() {

        }

        public string ResultName { get; set; }

        public string ResultListID { get; set; }

        public bool Primary { get; set; }

        public bool DefaultScoreboard { get; set; }

        public bool Team { get; set; }

        public string Status { get; set; }
    }
}

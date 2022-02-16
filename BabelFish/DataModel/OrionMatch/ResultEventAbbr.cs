using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultEventAbbr {

        public ResultEventAbbr() {
            ResultLists = new List<ResultListAbbr>();
        }

        public string DisplayName { get; set; }
        
        public List<ResultListAbbr> ResultLists { get; set; }

        public string EventName { get; set; }
    }
}

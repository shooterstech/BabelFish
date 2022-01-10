using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Match {


    [Serializable]
    public class MatchParticipantResult {

        public MatchParticipantResult() {
            UserID = null;
        }

        public string UserID { get; set; }

        public string EventName { get; set; }

        public string ResultCOFID { get; set; }
    }
}

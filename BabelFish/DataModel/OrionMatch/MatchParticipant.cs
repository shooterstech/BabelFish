using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class MatchParticipant {

        public MatchParticipant() {
            Participant = new Individual();
            RoleList = new List<string>();
            MatchParticipantResults = null;
        }

        public string MatchID { get; set; }

        public string ParentID { get; set; }

        public string UserID { get; set; }

        public string MatchName { get; set; }

        public string LocalDate { get; set; }

        public Participant Participant { get; set; }

        public List<MatchParticipantResult> MatchParticipantResults { get; set; }

        /// <summary>
        /// A list of Authorization Roles the participant has.
        /// </summary>
        public List<string> RoleList { get; set; }

        public override string ToString() {
            return "MatchParticipant for " + Participant.DisplayName;
        }
    }
}

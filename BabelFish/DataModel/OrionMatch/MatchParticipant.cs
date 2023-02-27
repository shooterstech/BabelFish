using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    [Serializable]
    public class MatchParticipant {

        public MatchParticipant() {
            Participant = new Individual();
            RoleList = new List<MatchParticipantRole>();
            MatchParticipantResults = new List<MatchParticipantResult>();
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (RoleList == null)
                RoleList = new List<MatchParticipantRole>();

            if (MatchParticipantResults == null)
                MatchParticipantResults= new List<MatchParticipantResult>();
        }

        /*
         * Currently choosing not to have theise properties within a MatchParticipant. This is because these properties 
         * are part of the MatchPartipantList.
         * 
        public string MatchID { get; set; }

        public string ParentID { get; set; }

        public string MatchName { get; set; }
        */
        public string LocalDate { get; set; }

        public Participant Participant { get; set; }

        public List<MatchParticipantResult> MatchParticipantResults { get; set; }

        /// <summary>
        /// A list of Authorization Roles the participant has.
        /// </summary>
        public List<MatchParticipantRole> RoleList { get; set; }

        public override string ToString() {
            return "MatchParticipant for " + Participant.DisplayName;
        }

        public string LastUpdated { get; set; }
    }
}

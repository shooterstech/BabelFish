using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json;


namespace Scopos.BabelFish.DataModel.OrionMatch {

    [Serializable]
    public class MatchParticipant : IParticipant {

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


        public string MatchID { get; set; }

        public string MatchName { get; set; }

        public string ParentID { get; set; }

        /// <summary>
        /// Unique ID within this match, for this Match Participant.
        /// </summary>
        public string ParticipantID { get; set; }

        /// <summary>
        /// UUID formatted Orion Account user id. 
        /// </summary>
        public string UserID { get; set; }

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

        public DateTime LastUpdated { get; set; }

		/// <summary>
		/// String holding the software (Orion Scoring System) and Version number of the software.
		/// </summary>
		public string Creator { get; set; }
	}
}

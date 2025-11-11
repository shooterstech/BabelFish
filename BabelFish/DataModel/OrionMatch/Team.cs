using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Any Team, or group of Participants in a Match. The group of Participants can be athletes or other teams
    /// </summary>
    [Serializable]
    public class Team : Participant {

        public const int CONCRETE_CLASS_ID = 2;

        public Team() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
            this.TeamMembers = new List<Participant>();
            this.TeamCaptains = new List<Individual>();
        }

        /// <summary>
        /// The contributing team members. These are the Participants that will make up the score shot by the team. 
        /// </summary>
        public List<Participant> TeamMembers { get; set; }

        /// <summary>
        /// The designated team captains for this team. A Team captain may also be a coach and may also be a member.
        /// </summary>
        public List<Individual> TeamCaptains { get; set; }

        /// <inheritdoc/>
        public override int UniqueMergeId {
            get {
                return this.DisplayName.GetHashCode();
            }
        }
    }
}

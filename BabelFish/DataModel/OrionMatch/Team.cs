using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Any Team, or group of Participants in a Match. The group of Participants can be athletes or other teams
    /// </summary>
    [Serializable]
    public class Team : Participant
    {
        public Team() {
            this.TeamMembers = new List<Participant>();
        }

        public List<Participant> TeamMembers { get; set; }
    }
}

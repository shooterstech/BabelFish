using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A list of roles one Loged in user, identified by their OrionID, has in the match.
    /// </summary>
    [Serializable]
    public class MatchAuthorization {
        public MatchAuthorization() {
        }

        public MatchAuthorization(string email, MatchParticipantRole role ) {
            UserEmail = email;
            Role = role;
        }

        /// <summary>
        /// email address of the authorized user
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// The authorization role given to this user.
        /// </summary>
        public MatchParticipantRole Role { get; set; }
    }
}

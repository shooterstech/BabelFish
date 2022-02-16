using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A list of roles one Loged in user, identified by their OrionID, has in the match.
    /// </summary>
    [Serializable]
    public class MatchAuthorization {
        public MatchAuthorization() {
        }

        public MatchAuthorization(string email, string role) {
            UserEmail = email;
            Role = role;
        }

        /// <summary>
        /// email address of the authorized user
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// The role the participant is assigned. If a participant has multiple roles use multiple MatchAuthorization Valid values are
        /// Match Director
        /// Range Officer
        /// Chief Range Officer
        /// Stat Officer
        /// Chief Stat Officer
        /// Jury Member
        /// Jury Chairmen
        /// Athlete
        /// Coach
        /// Team Official
        /// </summary>
        public string Role { get; set; }
    }
}

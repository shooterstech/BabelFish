using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class RoleAuthorization {

        /*
         * EKA NOTE November 2025
         * Defining RoleAuthorizagtion as an object for future possible expansion.
         */

        /// <summary>
        /// Public constructor.
        /// </summary>
        public RoleAuthorization() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="role">The MatchParticipantRole this object should have.</param>
        public RoleAuthorization( MatchParticipantRole role ) {
            this.Role = role;
        }


        /// <summary>
        /// The Match Participant Role
        /// </summary>
        [G_NS.JsonProperty( DefaultValueHandling = DefaultValueHandling.Include )]
        public MatchParticipantRole Role { get; set; } = MatchParticipantRole.NONE;
    }
}

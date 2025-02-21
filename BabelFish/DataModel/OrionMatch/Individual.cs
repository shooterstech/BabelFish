using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Any Individual person who is participating in a match. These include athletes, coaches,
    /// and match officials. Does not include Teams.
    /// </summary>
    [Serializable]
    public class Individual : Participant {

        public const int CONCRETE_CLASS_ID = 1;

        public Individual() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// Individual's Given or First Name
        /// </summary>
        public string GivenName { get; set; } = string.Empty;

        [JsonIgnore]
        public string FirstName {
            get { return this.GivenName; }
            set { this.GivenName = value; }
        }

        /// <summary>
        /// Individual's middle or secondary Given name
        /// </summary>
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Individual's Family or Last name
        /// </summary>
        public string FamilyName { get; set; } = string.Empty;

        public string LastName {
            get { return this.FamilyName; }
            set { this.FamilyName = value; }
        }

        /// <summary>
        /// If the Individual has a Scopos account, this is their unique identifier. Formatted as a UUID. A value null or empty string means the Indivdual does not have a Scopos account, or the Id is not known.
        /// </summary>
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier that represents the score (result cof object) this Individual had in this match. 
        /// </summary>
        [Obsolete( "Currently Orion only supports one Course of Fire per match. Once Orion supports multiple Courses of Fire this property will be removed and replaced with MatchParticipant.MatchParticipantResults.")]
        public string ResultCOFID { get; set;} = string.Empty;
    }
}

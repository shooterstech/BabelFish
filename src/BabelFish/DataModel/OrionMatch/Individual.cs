using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Any Individual person who is participating in a match. These include athletes, coaches,
    /// and match officials. Does not include Teams.
    /// </summary>
    [Serializable]
    public class Individual : Participant {

        public const int CONCRETE_CLASS_ID = 1;

        /// <summary>
        /// Default public constructor
        /// </summary>
        public Individual() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// Individual's Family or Last name
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public string FamilyName { get; set; } = string.Empty;

        /// <summary>
        /// Individual's Given or First Name
        /// </summary>
        [G_NS.JsonProperty( Order = 6 )]
        public string GivenName { get; set; } = string.Empty;

        /// <summary>
        /// Individual's middle or secondary Given name
        /// </summary>
        [G_NS.JsonProperty( Order = 7 )]
        [DefaultValue( "" )]
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// If the Individual has a Scopos account, this is their unique identifier. Formatted as a UUID. A value null or empty string means the Indivdual does not have a Scopos account, or the Id is not known.
        /// </summary>
        [G_NS.JsonProperty( Order = 9 )]
        [DefaultValue( "" )]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// The list of Match Authorization Roles this Individual has for a match.
        /// </summary>
        [G_NS.JsonProperty( Order = 25 )]
        public RoleList RoleList { get; set; } = new RoleList();

        /// <summary>
        /// NewtonSoft helper method to determine when to serialize .RoleList.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRoleList() {
            return RoleList != null && RoleList.Count > 0;
        }

        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        [Obsolete( "Use FamilyName" )]
        public string LastName {
            get { return this.FamilyName; }
            set { this.FamilyName = value; }
        }

        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        [Obsolete( "Use GivenName" )]
        public string FirstName {
            get { return this.GivenName; }
            set { this.GivenName = value; }
        }

        /// <summary>
        /// The unique identifier that represents the score (result cof object) this Individual had in this match. 
        /// </summary>
        [Obsolete( "Currently Orion only supports one Course of Fire per match. Once Orion supports multiple Courses of Fire this property will be removed and replaced with MatchParticipant.MatchParticipantResults.")]
        public string ResultCOFID { get; set;} = string.Empty;

        /// <inheritdoc />
        public override int UniqueMergeId {
            get {
                if (!string.IsNullOrEmpty( UserID ))
                    return this.UserID.GetHashCode();
                else 
                    return this.DisplayName.ToUpper().Trim().GetHashCode();
            }
        }
    }
}

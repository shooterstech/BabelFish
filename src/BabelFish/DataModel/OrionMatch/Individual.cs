using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Any Individual person who is participating in a match. These include athletes, coaches,
    /// and match officials. Does not include Teams.
    /// </summary>
    [Serializable]
    public class Individual : Participant {

        public const int CONCRETE_CLASS_ID = 1;

        #region Private and Protected Fields
        string _familyName = string.Empty;
        string _givenName = string.Empty;
        string _middleName = string.Empty;
        #endregion

        #region Constructors, Facory Methods, and Initialization Methods
        /// <summary>
        /// Default public constructor. Sets the correct value for ParticipantType and ConcreteClassId, which are used during deserialization to determine which concrete class to deserialize to, either a <see cref="Individual"/> or a <see cref="Team"/>.
        /// </summary>
        public Individual() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
            ParticipantType = ParticipantType.INDIVIDUAL;
        }
        #endregion

        #region Data Model Properties

        /// <summary>
        /// Individual's Family or Last name
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public string FamilyName {
            get { return _familyName; }
            set {
                _familyName = value.Trim();
                if (this.DefaultDisplayName)
                    SetDefaultDisplayName();
            }
        }

        /// <summary>
        /// Individual's Given or First Name
        /// </summary>
        [G_NS.JsonProperty( Order = 6 )]
        public string GivenName {
            get { return _givenName; }
            set {
                _givenName = value.Trim();
                if (this.DefaultDisplayName)
                    SetDefaultDisplayName();
            }
        }

        /// <summary>
        /// Individual's middle or secondary Given name
        /// </summary>
        [G_NS.JsonProperty( Order = 7 )]
        [DefaultValue( "" )]
        public string MiddleName {
            get { return _middleName; }
            set {
                _middleName = value.Trim();
                if (this.DefaultDisplayName)
                    SetDefaultDisplayName();
            }
        }

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
        [Obsolete( "Currently Orion only supports one Course of Fire per match. Once Orion supports multiple Courses of Fire this property will be removed and replaced with MatchParticipant.MatchParticipantResults." )]
        public string ResultCOFID { get; set; } = string.Empty;

        #endregion

        #region Methods

        public override void SetDefaultDisplayName() {

            this.DefaultDisplayName = true;

            if (!string.IsNullOrEmpty( GivenName ) && !string.IsNullOrEmpty( FamilyName )) {
                this._displayName = $"{FamilyName}, {GivenName}";
            } else if (!string.IsNullOrEmpty( GivenName )) {
                this._displayName = GivenName;
            } else if (!string.IsNullOrEmpty( FamilyName )) {
                this._displayName = FamilyName;
            } else {
                this._displayName = string.Empty;
            }
        }

        public override string GetDisplayNameShort() {

            //The rule of thumb is the DisplayNameShort should be 20 characters or less if possible.
            if (this._displayName.Length <= Individual.DISPLAY_NAME_SHORT_MAX_LENGTH) {
                return this._displayName;
            } else {

                if (!string.IsNullOrEmpty( GivenName ) && !string.IsNullOrEmpty( FamilyName )) {
                    if (FamilyName.Length <= (Individual.DISPLAY_NAME_SHORT_MAX_LENGTH - 2)) {
                        return $"{FamilyName} {GivenName.Substring( 0, 1 )}";
                    } else {
                        return $"{StringFormatting.GetTruncatedString( FamilyName, Individual.DISPLAY_NAME_SHORT_MAX_LENGTH - 2 )} {GivenName.Substring( 0, 1 )}";
                    }

                } else if (!string.IsNullOrEmpty( GivenName )) {
                    return StringFormatting.GetTruncatedString( GivenName, Individual.DISPLAY_NAME_SHORT_MAX_LENGTH );
                } else if (!string.IsNullOrEmpty( FamilyName )) {
                    return StringFormatting.GetTruncatedString( FamilyName, Individual.DISPLAY_NAME_SHORT_MAX_LENGTH );
                }
            }

            return string.Empty;
        }
        #endregion

        /// <inheritdoc />
        [G_NS.JsonIgnore]
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

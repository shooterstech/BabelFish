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
        [Obsolete( "Replaced with MatchParticipant.Entries[i].ResultCofId with the BabelFish 2.0 datamodel. Deprecated April 2026" )]
        [DefaultValue( "" )]
        public string ResultCOFID { get; set; } = string.Empty;

        #endregion

        #region Methods
        /// <inheritdoc />
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


        /// <inheritdoc />
        public override string GetDisplayNameShort( int maxLength = DISPLAY_NAME_SHORT_MAX_LENGTH ) {
            if (maxLength < DISPLAY_NAME_EXTRA_SHORT_MAX_LENGTH)
                maxLength = DISPLAY_NAME_EXTRA_SHORT_MAX_LENGTH;

            // First check the length of the DisplayName. If it is shorter than the max length, return it.
            if (this.DisplayName.Length <= maxLength) {
                return this.DisplayName;
            }

            // Next check the length of the DisplayNameShort. If it is shorter than the max length, return it.
            if (!string.IsNullOrWhiteSpace( this.DisplayNameShort ) && this.DisplayNameShort.Length <= maxLength) {
                return this.DisplayNameShort;
            }

            // Next check the length of the DisplayNameExtraShort. If it is shorter than the max length, return it.
            if (!string.IsNullOrWhiteSpace( this.DisplayNameExtraShort ) && this.DisplayNameExtraShort.Length <= maxLength) {
                return this.DisplayNameExtraShort;
            }

            // Attempt to generate a short display name using the FamilyName and GivenName. If it is shorter than the max length, return it.
            if (!string.IsNullOrEmpty( GivenName ) && !string.IsNullOrEmpty( FamilyName )) {
                string shortDisplayNameOpt1 = $"{FamilyName} {GivenName.Substring( 0, 1 )}";
                string shortDisplayNameOpt2 = $"{FamilyName}";
                if (shortDisplayNameOpt1.Length <= maxLength) {
                    return shortDisplayNameOpt1;
                } else if (shortDisplayNameOpt2.Length <= maxLength) {
                    return shortDisplayNameOpt2;
                }

            }

            if (!string.IsNullOrEmpty( FamilyName )) {
                return StringFormatting.GetTruncatedString( FamilyName, maxLength );
            }

            if (!string.IsNullOrEmpty( GivenName )) {
                return StringFormatting.GetTruncatedString( GivenName, maxLength );
            }

            return "Unknown";
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


        /// <inheritdoc />
        public override ulong CalculateChecksum() {
            var combined = $"{DisplayName}|{GivenName}|{MiddleName}|{FamilyName}|{CompetitorNumber}|{Country}|{HomeTown}|{Club}|{TeamName}";
            var hash = Helpers.Common.Md5ToUlong( combined );

            foreach (var attributeValue in AttributeValues) {
                hash ^= attributeValue.CalculateChecksum();
            }

            hash ^= RemarkList.CalculateChecksum();

            return hash;
        }
    }
}

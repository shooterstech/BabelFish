using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// A Participant is anyone who has a role in a Match. This includes athletes, teams, match officials, and coaches.
    /// 
    /// IMPORTANT: When adding Participant to a class (such as Result COF or ResultEvent (under Result List), need to make 
    /// sure to deserialize the Participant's attribute values. To do so, as an example, see GetResultCOFResponse's 
    /// PostResponseProcessingAsync()
    /// </summary>
    [Serializable]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ParticipantConverter ) )]
    public abstract class Participant :
        G_STJ_SER.IJsonOnDeserializing,
        G_STJ_SER.IJsonOnDeserialized {

        /// <summary>
        /// The expected maximum length of the DisplayNameShort property. This is not a hard limit, but by convention DisplayNameShorts should be 20 characters or less. DisplayName may be any length.
        /// </summary>
        public const int DISPLAY_NAME_SHORT_MAX_LENGTH = 20;

        #region Private and Protected Fields
        protected string _displayName = string.Empty;
        protected bool _ignoreEvents = false;
        #endregion

        #region Constructors, Factory Methods, and Initialization
        public Participant() {
            Coaches = new List<Individual>();
        }

        public void OnDeserialized() {
            _ignoreEvents = false;
        }

        public void OnDeserializing() {
            _ignoreEvents = true;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Eventhanderl that gets invoked with DisplayName changes. The event args include the Participant whose DisplayName changed, and the PreviousDisplayName property can be used to get the previous value of DisplayName.
        /// </summary>
        [G_NS.JsonIgnore]
        public EventHandler<EventArgs<Participant>> OnDisplayNameChanged;

        #endregion

        #region Data Properties
        /// <summary>
        /// When a competitor's name is displayed, this is the value that is displayed vy default.
        /// <para>Alternatively, on a reduced width screen a shorter display name is returned using <see cref="GetDisplayNameShort()"/>.</para>
        /// <para>By default, the DisplayName is calculated based on other properties of the Participant, such as FamilyName and GivenName for an Individual, or TeamName for a Team.
        /// If those properties are modified after the DisplayName is set, the DisplayName will not automatically update to reflect those changes.
        /// If DisplayName is set outside of these other properties, then the value is no longer calculated and only the value it is set to is returned.</para>
        /// <para>To get notified when DisplayName changes, subscribe to the <see cref="OnDisplayNameChanged"/> event.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string DisplayName {
            get {
                return _displayName;
            }
            set {
                PreviousDisplayName = _displayName;
                _displayName = value;

                if (!_ignoreEvents) {
                    //This code will not run during deserialization, since _ignoreEvents is set to true during deserialization.
                    //This is intentional, as we don't want to trigger DisplayName change events during deserialization.
                    DefaultDisplayName = false;
                    OnDisplayNameChanged?.Invoke( this, new EventArgs<Participant>( this ) );
                }
            }
        }

        /// <summary>
        /// Specifies if the DisplayName is the default value. Would be true if the user hasn't modified the DisplayName. Would be false if the user has set the DisplayName.
        /// <para>It is generally not recommended to manually set this property. It is managed automatically based on whether the DisplayName has been modified by the user.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = G_NS.DefaultValueHandling.IgnoreAndPopulate )]
        [DefaultValue( true )]
        public bool DefaultDisplayName { get; set; } = true;

        /// <summary>
        /// When the DisplayName is changed, this property holds the previous value of DisplayName.
        /// Often event handlers will want to know both the new and old display name, so this property is included for that purpose.
        /// This value is not serialized, and is only intended to be used during event handling of DisplayName changes. 
        /// </summary>
        [G_NS.JsonIgnore]
        public string PreviousDisplayName { get; private set; } = string.Empty;

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [Obsolete( "Use ParticipantType. Deprecated March 2026" )]
        public int ConcreteClassId { get; set; }

        /// <summary>
        /// Concrete class identifying the type of Participant.
        /// This is used during deserialization to determine which concrete class to deserialize to, either a <see cref="Individual"/> or a <see cref="Team"/>.
        /// </summary>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public ParticipantType ParticipantType { get; set; } = ParticipantType.INDIVIDUAL;

        /*
         * JsonProperty Order values 5 .. 9 reserved for concrete classes
         */

        /// <summary>
        /// A unique, human readable, value assigned to all Participants in a match.
        /// 
        /// In most cases the CompetitorNumber will be numeric, but it can also be alphabetical.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        [DefaultValue( "" )]
        public string CompetitorNumber { get; set; } = string.Empty;

        /// <summary>
        /// The three letter country code the participant is from.
        /// </summary>
        [G_NS.JsonProperty( Order = 11 )]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// The Hometown the participant is from.
        /// </summary>
        [G_NS.JsonProperty( Order = 12 )]
        [DefaultValue( "" )]
        public string HomeTown { get; set; } = string.Empty;

        //TODO: Club, ReentryTag not in API return data
        /// <summary>
        /// The Hometown Club the Participant represents. Note, this is NOT the same as any team the Participant is shooting with. 
        /// </summary>
        [G_NS.JsonProperty( Order = 13 )]
        [DefaultValue( "" )]
        public string Club { get; set; } = string.Empty;

        /// <summary>
        /// The <see cref="Team"/> that this Participant is a member of. A value of null
        /// indicates that the Participant is not a member of any team. 
        /// <para>Note, this is NOT the same as the Club property, which represents the hometown club the Participant represents.</para>
        /// </summary>
        /// <remark>
        /// Value is not serialized, as this is a pointer back to the container Team.
        /// </remark>
        [G_NS.JsonIgnore]
        public Team? Team { get; set; } = null;

        /// <summary>
        /// Gets the name of the team associated with the current participant.
        /// </summary>
        /// <remarks>If no team is assigned, this property returns an empty string. Use this property to
        /// retrieve the display name of the participant's team, if available.</remarks>
        [G_NS.JsonProperty( Order = 15 )]
        public virtual string TeamName {
            get {
                return Team != null ? Team.TeamName : string.Empty;
            }
            set {
                ; // an Individual doesn't have a team name, so the setter does nothing. For a Team, the TeamName is the same as the DisplayName, so the setter of TeamName sets the DisplayName.
            }
        }

        /*
         * JsonProperty Order values 16 .. 19 reserved for concrete classes
         */

        /// <summary>
        /// A list of AttributeValues assigned to this Participant.
        /// </summary>
        [G_NS.JsonProperty( Order = 21 )]
        public List<AttributeValueDataPacketMatch> AttributeValues { get; set; } = new List<AttributeValueDataPacketMatch>();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize AttributeValues when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeAttributeValues() {
            return (AttributeValues != null && AttributeValues.Count > 0);
        }

        /// <summary>
        /// A list of Remark objects, each containing a RemarkName, sometimes a reason, and a status (show or don't)
        /// </summary>
        /// </remarks>
        [Obsolete( "RemarkList is now implemented as part of the CourseOfFireEntry class. As each Course of Fire a participants shoots may have a different RemarkList. Deprecated April 2025." )]
        [G_NS.JsonProperty( Order = 22 )]
        public RemarkList RemarkList { get; set; } = new RemarkList();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize AttributeValues when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRemarkList() {
            return (RemarkList != null && RemarkList.Count() > 0);
        }

        /// <summary>
        /// A list of this Participant's coaches.
        /// </summary>
        [G_NS.JsonProperty( Order = 23 )]
        public List<Individual> Coaches { get; set; }

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize Coaches when the list has something in it.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeCoaches() {
            return (Coaches != null && Coaches.Count > 0);
        }

        /*
         * JsonProperty Order values 25 .. 29 reserved for concrete classes
         */

        /// <summary>
        /// The Version string of the JSON document.
        /// Version 2022-04-09 represents ResultCOF in a dictionary format
        /// Version < 2022 represent ResultCOF in a tree format
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 98 )]
        [G_NS.JsonProperty( Order = 98 )]
        public string JSONVersion { get; set; } = string.Empty;

        /// <summary>
        /// UTC time the match data was last updated.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 99 )]
        [G_NS.JsonProperty( Order = 99 )]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        #endregion

        #region Methods

        /// <summary>
        /// Calling this method sets (or restores) the value of <see cref="DisplayName"/> based on the values of other properties of the Participant, such as FamilyName and GivenName for an Individual, or TeamName for a Team.
        /// It also marks that the DisplayName is the default value by setting <see cref="DefaultDisplayName"/> to true. 
        /// </summary>
        /// <remarks>The concrete class implementers should set _displayNameSet and _displayNameShorSet to false, as this represents the user has not modified the default values of DisplayName yet.</remarks>
        public abstract void SetDefaultDisplayName();

        /// <summary>
        /// Calculates a shorter version of the <see cref="DisplayName"/> property. Intended to be used on screens of reduced width.
        /// <para>Concrete classes may override the <see cref="GetDisplayNameShort"/> method to provide a custom implementation.</para>
        /// <para>By convention, the shortened display name should be no longer than <see cref="DISPLAY_NAME_SHORT_MAX_LENGTH"/> (20) characters.</para>
        /// </summary>
        /// <returns></returns>
        public virtual string GetDisplayNameShort() {
            if (this.DisplayName.Length <= DISPLAY_NAME_SHORT_MAX_LENGTH) {
                return this.DisplayName;
            } else {
                return StringFormatting.GetTruncatedString( this.DisplayName, DISPLAY_NAME_SHORT_MAX_LENGTH );
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            return this.DisplayName;
        }

        #endregion

        #region IFinishInitializationAsync Implementation

        /// <inheritdoc />
        /// <remarks>The prefered method for deserializing from json is to use <see cref="MatchParticipant.LoadFromFileAsync(FileInfo)"/> or <see cref="MatchParticipant.LoadFromFileAsync(string)"/>.
        /// if you are deserializing outside of these methods besure to call FinishInitiializationAsync() before using your Participant object.</remarks>
        public async Task FinishInitializationAsync() {
            foreach (var attributeValue in this.AttributeValues) {
                await attributeValue.FinishInitializationAsync();
            }
        }

        #endregion

        /// <summary>
        /// Calculated value to use to identify the same particpant accross multiple result lists.
        /// Primarily used in TournamentMerge.
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public abstract int UniqueMergeId { get; }
    }
}

using System.ComponentModel;
using Scopos.BabelFish.DataModel.Definitions;

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

        /// <summary>
        /// Generates a deep copy of this Participant instance, with the notable exception of the participant's attribute values and Team Members.
        /// The AttributeValues list of the copied Participant is populated based on the provided CourseOfFireStructure, which includes
        /// both the Course of Fire specific attributes and the global attributes for the match.
        /// </summary>
        /// <param name="cofStructure"></param>
        /// <returns></returns>
        /// <remarks>This method is intended to be used in the generation of <see cref="ResultCOF"/> or <see cref="ResultEvent"/> instances (which are both considered
        /// 'compiled' data formats). Additionally, calling this method should be done within the scope of a <see cref="MatchProject"/>.</remarks>
        public async Task<Participant> CopyAsync( CourseOfFireStructure cofStructure ) {
            var copy = this.Clone(); //Using Clone is slow, but works

            if (this.MatchParticipant is null) {
                throw new BackwardsPointerException( $"The backwards pointer MatchParticipant is null for participant {this.DisplayName}. This should never happen during normal operation as part of a MatchProject. If this Participant exists outside of a MatchProject, then you shouldn't be calling Copy()." );
            }

            // Set the read only TeamName property if this participant is an Individual and is associated with a team for this course of fire. This is needed as part of the generation of ResultCOF and ResultEvent data structures.
            if (this is not Team
                && this.MatchParticipant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )
                && entry.Team is not null) {
                copy.TeamName = entry.Team.TeamName;
            }
            copy.AttributeValues = new List<AttributeValueDataPacketMatch>();

            // Populate the Participant Attribute Values specific to this Course of Fire. Which includes the attributes specific to the course of fire structure, and then the global attributes for the match.
            // By calling GetAttributeValueAsync we also include the correct values for Constant attributes.

            foreach (var attrConfig in cofStructure.Attributes) {
                var attrValue = await this.GetAttributeValueAsync( attrConfig.AttributeDef, cofStructure.CourseOfFireId );
                if (attrValue is not null) {
                    copy.AttributeValues.Add( attrValue );
                }
            }
            foreach (var attrConfig in cofStructure.MatchStructure.GlobalAttributes) {
                var attrValue = await this.GetAttributeValueAsync( attrConfig.AttributeDef, 0 );
                if (attrValue is not null) {
                    copy.AttributeValues.Add( attrValue );
                }
            }

            return copy;
        }

        /// <summary>
        /// Method that gets called after System.Text.Json deserializes an instance of this class. Sets _ignoreEvents to false to allow events to fire after deserialization.
        /// </summary>
        public void OnDeserialized() {
            _ignoreEvents = false;
        }

        /// <summary>
        /// Method that gets called before System.Text.Json deserializes an instance of this class. Sets _ignoreEvents to true to prevent events from firing during deserialization.
        /// </summary>
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

        #region Data Model Properties
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
        /// When serialized as an Individual and as part of a <see cref="ResultCOF"/> or <see cref="ResultEvent"/>, this property holds the name of the team that this
        /// Participant is shooting with for that Course of Fire. An empty string if they are not associated with a Team.
        /// <para>This property has no meaning outside of being serialized as part of a <see cref="ResultCOF"/> or <see cref="ResultEvent"/>.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 15 )]
        [DefaultValue( "" )]
        public virtual string TeamName { get; set; }

        /*
         * JsonProperty Order values 16 .. 19 reserved for concrete classes
         */

        /// <summary>
        /// A list of AttributeValues assigned to this Participant.
        /// <para>It should only contain non-Constant AttributeValues. Note a Constant AttributeValue is shared across all participants and is demarcated
        /// using <see cref="AttributeConfiguration.Constant"/> within either the <see cref="CourseOfFireStructure"/> (for Course of Fire specific Attributes)
        /// or <see cref="MatchStructure"/> for global attributes.</para>
        /// <para>The preferred way to get an Attribute Value for a Participant, either COF specific or global is to use
        /// <see cref="GetAttributeValueAsync"/>.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 21 )]
        public List<AttributeValueDataPacketMatch> AttributeValues { get; set; } = new List<AttributeValueDataPacketMatch>();

        /// <summary>
        /// A list of Remark objects, each containing a RemarkName, sometimes a reason, and a status (show or don't)
        /// </summary>
        /// </remarks>
        [Obsolete( "RemarkList is now implemented as part of the CourseOfFireEntry class. As each Course of Fire a participants shoots may have a different RemarkList. Deprecated April 2025." )]
        [G_NS.JsonProperty( Order = 22 )]
        public RemarkList RemarkList { get; set; } = new RemarkList();

        /// <summary>
        /// A list of this Participant's coaches.
        /// </summary>
        [G_NS.JsonProperty( Order = 23 )]
        public List<Individual> Coaches { get; set; }

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

        #region Helper Properties

        /// <summary>
        /// Backwards pointer to the MatchParticipant that this Participant is associated with. This is set during deserialization of the
        /// MatchParticipant, and is used to allow the Participant to access information about the match and other participants if needed.
        /// <para>Value might be null if this Participant was created or deserialized outside the context of a <see cref="MatchProject"/>.</para>
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchParticipant? MatchParticipant { get; internal set; }

        /// <summary>
        /// Helper property, read only, property to get the MatchProject that this Participant is associated with, through the MatchParticipant. 
        /// <para>Value might be null if this Participant was created or deserialized outside the context of a <see cref="MatchProject"/>,
        /// for example the GetMatchParticipantList API call.</para>
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchProject? MatchProject {
            get {
                return this.MatchParticipant?.Project ?? null;
            }
        }
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
        /// <summary>
        /// Gets the AttributeValue for a specific SetName and CourseOfFireId. 
        /// <para>To get global AttributeValues that apply to the whole match, use a CourseOfFireId of 0.</para>
        /// <para>If the Participant doesn't have an AttributeValue for the specified SetName and CourseOfFireId, but the Attribute
        /// is defined within the <see cref="MatchStructure"/>, a default AttributeValue is generated, stored for the participant
        /// and returned.</para>
        /// <para>null is returned otherwise.</para>
        /// </summary>
        /// <param name="setName">The SetName of the attribute.</param>
        /// <param name="courseOfFireId">The CourseOfFireId of the attribute.</param>
        /// <returns>The AttributeValueDataPacketMatch if found, otherwise null.</returns>
        /// <remarks>This method is asynchronous to allow for the possibility of needing to create default AttributeValues, but in most cases it will complete synchronously. The
        /// nearly equivalent synchronous method is <see cref="TryGetAttributeValue"/>.</remarks>
        public async Task<AttributeValueDataPacketMatch?> GetAttributeValueAsync( SetName setName, int courseOfFireId ) {

            return await Task.FromResult( GetAttributeValueInternal( setName, courseOfFireId,
                attributeConfiguration => {
                    // If not found, create a default AttributeValue
                    return AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ).GetAwaiter().GetResult();
                } ) );
        }

        /// <summary>
        /// Attempts to get the AttributeValue for a specific SetName and CourseOfFireId. Returns true if found, false otherwise. The result is returned through an out parameter.
        /// <para>A default value is not created by this method, if the attribute value is not found. To create a default value if not found, use <see cref="GetAttributeValueAsync"/>.</para>
        /// </summary>
        /// <param name="setName">The SetName of the attribute.</param>
        /// <param name="courseOfFireId">The CourseOfFireId of the attribute.</param>
        /// <param name="result">The AttributeValueDataPacketMatch if found, otherwise null.</param>
        /// <returns>True if the AttributeValueDataPacketMatch was found, otherwise false.</returns>
        public bool TryGetAttributeValue( SetName setName, int courseOfFireId, out AttributeValueDataPacketMatch? result ) {
            result = GetAttributeValueInternal(
                setName,
                courseOfFireId,
                ( attributeConfiguration ) => null // Don't create, just return null
            );
            return result != null;
        }

        private AttributeValueDataPacketMatch? GetAttributeValueInternal(
            SetName setName,
            int courseOfFireId,
            Func<AttributeConfiguration, AttributeValueDataPacketMatch?> createDefault ) {
            if (setName.IsDefault)
                return null;

            if (courseOfFireId > 0) {
                if (this.MatchProject?.Match?.MatchStructure?.TryGetCourseOfFireStructure( courseOfFireId, out var courseOfFire ) ?? false) {
                    foreach (var attributeConfiguration in courseOfFire.Attributes) {
                        if (attributeConfiguration.AttributeDef.Equals( setName )) {
                            if (attributeConfiguration.Constant)
                                return attributeConfiguration.GetAsAttributeValueDataPacketMatch();

                            foreach (var attributeValue in this.AttributeValues) {
                                if (attributeValue.AttributeDef.Equals( setName ) && attributeValue.CourseOfFireId == courseOfFireId)
                                    return attributeValue;
                            }

                            // If not found, use the provided delegate to handle creation
                            return createDefault( attributeConfiguration );
                        }
                    }
                }
            } else {
                foreach (var attributeConfiguration in this.MatchProject?.Match?.MatchStructure?.GlobalAttributes ?? Enumerable.Empty<AttributeConfiguration>()) {
                    if (attributeConfiguration.AttributeDef.Equals( setName )) {
                        if (attributeConfiguration.Constant)
                            return attributeConfiguration.GetAsAttributeValueDataPacketMatch();

                        foreach (var attributeValue in this.AttributeValues) {
                            if (attributeValue.AttributeDef.Equals( setName ) && attributeValue.CourseOfFireId == courseOfFireId)
                                return attributeValue;
                        }

                        // If not found, use the provided delegate to handle creation
                        return createDefault( attributeConfiguration );
                    }
                }
            }

            foreach (var attributeValue in this.AttributeValues) {
                if (attributeValue.AttributeDef.Equals( setName ) && attributeValue.CourseOfFireId == courseOfFireId)
                    return attributeValue;
            }

            return null;
        }

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize AttributeValues when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeAttributeValues() {
            return (AttributeValues != null && AttributeValues.Count > 0);
        }

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize Coaches when the list has something in it.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeCoaches() {
            return (Coaches != null && Coaches.Count > 0);
        }

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize AttributeValues when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRemarkList() {
            return (RemarkList != null && RemarkList.Count() > 0);
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

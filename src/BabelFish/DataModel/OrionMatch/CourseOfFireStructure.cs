using System.ComponentModel;
using System.Diagnostics;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    ///
    /// <para>It is generally best to construct a new instance using the <see cref="CreateAsync(SetName)"/> method.</para>
    /// </summary>
    /// <remarks>New with BabelFish 2.0 / Orion 3.0 DataModel</remarks>
    public class CourseOfFireStructure :
        IFinishInitializationAsync,
        IGetCourseOfFireDefinition,
        G_STJ_SER.IJsonOnDeserializing,
        G_STJ_SER.IJsonOnDeserialized {

        #region Private and Protected Fields
        protected bool _ignoreEvents = false;
        private bool _official = false;
        private int _numberOfTeamMembers = 4;
        private int _maxNumberOfTeamMembers = 100;
        #endregion

        #region Constructors, Facory Methods, and Initialization Methods
        /// <summary>
        /// Public constructor. Unless you are a deserializer, it is generally best to construct a new instance using the
        /// <see cref="CreateAsync(SetName)"/> method, which sets default values for properties based on the CourseOfFire definition.
        /// </summary>
        public CourseOfFireStructure() { }

        /// <summary>
        /// Creates a new instance of the CourseOfFireStructure class using the specified <see cref="Definitions.CourseOfFire"/> set name.
        /// </summary>
        /// <remarks>The created CourseOfFireStructure is initialized with properties from the provided
        /// match structure and course of fire definition. The start and end dates are set to the current date. If the
        /// course of fire definition specifies a required attribute configuration, it is added to the structure
        /// as well.</remarks>
        /// <param name="matchStructure">The match structure that this instance will be a part of.</param>
        /// <param name="setName">The name of the course of fire set to use for creation. This value must not be the default set name.</param>
        /// <returns>Returns the newly created CourseOfFireStructure instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified setname is the default</exception>
        /// <exception cref="DefinitionNotFoundException">Thrown when the provided setname is not a known COURSE OF FIRE definition.</exception>"
        public static async Task<CourseOfFireStructure> CreateAsync( MatchStructure matchStructure, SetName setName ) {

            if (setName.IsDefault)
                throw new ArgumentException( "May not add a new COURSE OF FIRE using default." );

            var cof = await DefinitionCache.GetCourseOfFireDefinitionAsync( setName );

            var structure = new CourseOfFireStructure();
            structure.MatchStructure = matchStructure;
            structure.CourseOfFireDef = setName;
            structure.CourseOfFireName = cof.CommonName;
            structure.StartDate = DateTime.Today;
            structure.EndDate = DateTime.Today;
            structure.ScoreConfigName = cof.ScoreConfigDefault;
            structure.TargetCollectionName = cof.DefaultTargetCollectionName;
            if (!cof.RequiredAttributeDef.IsDefault) {
                await structure.AddAttributeConfigurationAsync( cof.RequiredAttributeDef );
            }

            return structure;
        }

        /// <summary>
        /// Gets called after System.Text.Json deserializes an instance of this class.
        /// Sets _ignoreEvents to false so that events will fire as expected after deserialization.
        /// </summary>
        public void OnDeserialized() {
            foreach (var rl in ResultLists) {
                rl.CourseOfFireId = this.CourseOfFireId;
                rl.AttributeFilter.UpdateCourseOfFireId( this.CourseOfFireId );
            }
            _ignoreEvents = false;
        }

        /// <summary>
        /// Gets called before System.Text.Json deserializes an instance of this class.
        /// Sets _ignoreEvents to true to prevent events from firing during deserialization.
        /// </summary>
        public void OnDeserializing() {
            _ignoreEvents = true;
        }

        /// <inheritdoc />
        public async Task FinishInitializationAsync() {
            foreach (var localAttribute in Attributes) {
                await localAttribute.FinishInitializationAsync();
            }
            foreach (var rl in ResultLists) {
                await rl.FinishInitializationAsync();
            }
        }
        #endregion


        #region Events
        /// <summary>
        /// Occurs when a new <see cref="ResultListAbbr"/> is added.
        /// <para>The preferred way of adding a new ResultListAbbr is to use the <see cref="AddResultList(ResultListAbbr)"/> method.
        /// Adding a ResultListAbbr directly to <see cref="ResultLists"/> will not result in this event handler being fired.</para>
        /// </summary>
        [G_NS.JsonIgnore]
        EventHandler<EventArgs<ResultListAbbr>> OnResultListAdded;

        /// <summary>
        /// Occurs when a new <see cref="AttributeConfiguration"/> is added.
        /// <para>The preferred way of adding a new AttributeConfiguration is to use the <see cref="AddAttributeConfigurationAsync(SetName)"/> method.
        /// Adding an AttributeConfiguration directly to <see cref="Attributes"/> will not result in this event handler being fired.</para>
        /// </summary>
        [G_NS.JsonIgnore]
        EventHandler<EventArgs<AttributeConfiguration>> OnAttributeConfigurationAdded;
        #endregion

        #region Data Model Properties

        /// <summary>
        /// Human readable name given to this CourseOfFireStruccture. It is generally best if it is unique
        /// within a <see cref="Match"/>, but not required.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string CourseOfFireName { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier, usually incremented, within a <see cref="Match"/>
        /// <para>The value of 0 is reserved and may not be used. </para>
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public int CourseOfFireId { get; set; } = 1;

        /// <summary>
        /// The SetName of the <see cref="CourseOfFire"/> definition that this CourseOfFireStructure is based on.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public SetName CourseOfFireDef { get; set; } = SetName.Parse( "v1.0:orion:Default" );

        /// <summary>
        /// The start date of this Course of Fire.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 4 )]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// the end date of this Course of fire.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 5 )]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicator of whether this Course of Fire is official. The value of this property is always true if the
        /// current date is past the end date of the Course of Fire, and is otherwise determined by the value set to this property.
        /// </summary>
        /// <remarks>Unlike a Result List, a COF Structure can only be not-official or official (A result list may be future, intermediate,
        /// unofficial, or official). The designation of official means the Match Director has blessed the results and says everythign is done.
        /// </remarks>
        [G_NS.JsonProperty( Order = 10 )]
        public bool Official {
            get {
                if (DateTime.Today > EndDate) {
                    return true;
                }
                return _official;
            }
            set {
                _official = value;
            }
        }

        /// <summary>
        /// Human readable description for this Course of Fire.
        /// </summary>
        [G_NS.JsonProperty( Order = 11 )]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Name of the ScoreConfig used in this match.
        /// </summary>
        /// <remarks>The name of the SCORE FORMAT COLLECTION is specified in the COUSE OF FIRE. </remarks>
        [G_NS.JsonProperty( Order = 12 )]
        public string ScoreConfigName { get; set; }

        /// <summary>
        /// Name of the TargetCollection used in this Course of Fire.
        /// </summary>
        /// <remarks>The TARGET COLLECTION is specified in the COURSE OF FIRE. </remarks>
        [G_NS.JsonProperty( Order = 13 )]
        public string TargetCollectionName { get; set; }

        /// <summary>
        /// The name of the <see cref="PaperTargetLabel"/> to use for this Course of Fire. Value will drive
        /// how barcode labels are printed and how many shots per aiming bull target reading machines are
        /// expecting to find when shot on paper.
        /// <para>Value is not required. If shooting on paper targets, the first <see cref="PaperTargetLabel"/>
        /// listed in the <see cref="CourseOfFireDef"/> is used as the default.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 14 )]
        [DefaultValue( "" )]
        public string PaperTargetLabelName { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the algorithm to use to project INTERMEDIATE score.
        /// The default value is AVERAGE_SHOT_FIRED which means it will use the <see cref="ProjectScoresByAverageShotFired"/> class.
        /// </summary>
        [G_NS.JsonProperty( Order = 15 )]
        public ProjectorOfScoresType ProjectorOfScores { get; set; } = ProjectorOfScoresType.AVERAGE_SHOT_FIRED;

        /// <summary>
        /// Gets or sets the types of entries that can be recorded, which may include individual and team entries.
        /// </summary>
        [G_NS.JsonProperty( Order = 16, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public EntryTypes TypesOfEntries { get; set; } = EntryTypes.INDIVIDUAL_AND_TEAM;

        /// <summary>
        /// The list of AttributeConfigurations that are specific to this Course of Fire.
        /// These attributes will be available to be added to entries in this Course of Fire, but not entries in other Courses of Fire in the same match.
        /// If there are attributes that should be shared across all Courses of Fire in a match, those should be added
        /// to the <see cref="MatchStructure.GlobalAttributes"/> collection instead.
        /// <para>When adding a new AttributeConfiguration to this collection, it is generally best to use the
        /// <see cref="AddAttributeConfigurationAsync(SetName)"/> method which adds the attribute to each
        /// exisitng entry in the match.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 20 )]
        public List<AttributeConfiguration> Attributes { get; set; } = new List<AttributeConfiguration>();

        /// <summary>
        /// Gets or sets the collection of result events represented by abbreviated result lists.
        /// <para>The preferred method of adding a new ResultListAbbr is by calling <see cref="AddResultList(ResultListAbbr)"/> which checks for duplicates before adding.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 21 )]
        public List<ResultListAbbr> ResultLists { get; set; } = new List<ResultListAbbr>();

        /// <summary>
        /// Gets and sets the number of Participants that contribute to a Team's score for this Course of Fire.
        /// This property is only relevant if <see cref="TypesOfEntries"/> is set to allow team entries. The default value is 4.
        /// <para>If the value is set to a number greater than <see cref="MaxNumberOfTeamMembers"/>, the <see cref="MaxNumberOfTeamMembers"/> property will be updated accordingly.</para>
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the provided value is less than 2 (as that wouldn't be a team).</exception>
        /// <remarks>NumberOfTeamMembers and MaxNumberOfTeamMembers differ in that MaxNumberOfTeamMembers is the number of
        /// participants that may be allowed to make up a team. Number of TeamMembers is the number of participants who's score
        /// counts towards the team total.</remarks>
        [G_NS.JsonProperty( Order = 30 )]
        public int NumberOfTeamMembers {
            get {
                return _numberOfTeamMembers;
            }
            set {
                if (value < 2) {
                    throw new ArgumentException( "Number of team members must be at least 2." );
                }
                _numberOfTeamMembers = value;
                if (value > MaxNumberOfTeamMembers) {
                    MaxNumberOfTeamMembers = _numberOfTeamMembers;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of team members allowed to be members of a team within this Course of Fire.
        /// This property is only relevant if <see cref="TypesOfEntries"/> is set to allow team entries. The default value is 100.
        /// Value must be greater than or equal to <see cref="NumberOfTeamMembers"/>, this rule is enforced during the setting process.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the provided value is less than 2 (as that wouldn't be a team).</exception>
        /// <remarks>NumberOfTeamMembers and MaxNumberOfTeamMembers differ in that MaxNumberOfTeamMembers is the number of
        /// participants that may be allowed to make up a team. Number of TeamMembers is the number of participants who's score
        /// counts towards the team total.</remarks>
        [G_NS.JsonProperty( Order = 31 )]
        public int MaxNumberOfTeamMembers {
            get { return _maxNumberOfTeamMembers; }
            set {
                if (value < 2) {
                    throw new ArgumentException( "Max number of team members must be at least 2." );
                }
                _maxNumberOfTeamMembers = Math.Max( NumberOfTeamMembers, value );
            }
        }

        /// <summary>
        /// Data about how this Course of Fire was competed, such as the scoring systems in use.
        /// <para>This is considered compiled data. Does not contain any configuration information.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 32 )]
        public CourseOfFireMetaData MetaData { get; set; } = new CourseOfFireMetaData();
        #endregion

        #region Helper Properties
        /// <summary>
        /// Readonly, backwares pointer to the MatchStructure that holds this CourseOfFireStructure.
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchStructure MatchStructure { get; internal set; }

        /// <summary>
        /// REadonly, runtime property to temporairly disable score projection. 
        /// </summary>
        [G_NS.JsonIgnore]
        public bool DisableScoreProjection { get; internal set; } = false;
        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified result list to the ResultLists collection, checking that it is not already a member.
        /// </summary>
        /// <param name="resultList">The result list to add to the collection. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided resultList is null.</exception>"
        public bool AddResultList( ResultListAbbr? resultList ) {
            if (resultList is null) {
                throw new ArgumentNullException( nameof( resultList ) );
            }

            var existingResultList = ResultLists.Find( x => x.GetHashCode() == resultList.GetHashCode() );

            if (existingResultList == null) {
                ResultLists.Add( resultList );

                resultList.CourseOfFireId = this.CourseOfFireId;
                resultList.AttributeFilter.UpdateCourseOfFireId( this.CourseOfFireId );

                if (!_ignoreEvents) {
                    OnResultListAdded?.Invoke( this, new EventArgs<ResultListAbbr>( resultList ) );
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds each of the passed in ResultLists to this CourseOfFireStructure. If the ResultList is already a member of the CourseOfFireStructure, it is not added again.
        /// After a ResultList is successfully added, the <see cref="OnResultListAdded"/> event is fired.
        /// </summary>
        /// <param name="resultLists"></param>
        public void AddResultList( IEnumerable<ResultListAbbr> resultLists ) {
            foreach (var rl in resultLists) {
                AddResultList( rl );
            }
        }

        /// <summary>
        /// The preferred method of adding a new AttributeConfiguration to this CourseOfFireStructure. This method creates a new AttributeConfiguration based on the provided set name and adds it to the Attributes collection.
        /// </summary>
        /// <param name="attributeDef"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception></para>
        public async Task<AttributeConfiguration> AddAttributeConfigurationAsync( SetName attributeDef ) {

            if (attributeDef.IsDefault) {

                throw new ArgumentException( "May not add an AttributeConfiguration using default." );
            }

            var attributeConfig = await AttributeConfiguration.CreateAsync( attributeDef );
            attributeConfig.CourseOfFireId = this.CourseOfFireId;
            Attributes.Add( attributeConfig );

            if (MatchStructure is not null && MatchStructure.Match is not null && MatchStructure.Match.MatchProject is not null) {
                foreach (var mp in MatchStructure.Match.MatchProject.Participants) {
                    if (attributeConfig.IsForIndividuals && mp.Participant.ParticipantType == ParticipantType.INDIVIDUAL) {
                        var avdp = await AttributeValueDataPacketMatch.CreateAsync( attributeConfig );
                        avdp.CourseOfFireId = this.CourseOfFireId;
                        mp.Participant.AttributeValues.Add( avdp );
                    }
                }
            } else {
                Debug.Assert( MatchStructure is not null, "The MatchStructure property of this CourseOfFireStructure is null. Likely means it was not set when this instance was created or deserialized." );
                Debug.Assert( MatchStructure.Match is not null, "The Match property of the MatchStructure property of this CourseOfFireStructure is null. Likely means it was not set when this instance was created or deserialized." );

                //The following assertion is not valid because a Match instance could live in its own file and be deserialized separately from the Project instance that contains it
                //Debug.Assert( MatchStructure.Match.Project is not null, "The Project property of the Match property of the MatchStructure property of this CourseOfFireStructure is null. Likely means it was not set when this instance was created or deserialized." );
            }

            if (!_ignoreEvents) {
                OnAttributeConfigurationAdded?.Invoke( this, new EventArgs<AttributeConfiguration>( attributeConfig ) );
            }

            return attributeConfig;
        }

        /// <inheritdoc />
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {
            return await DefinitionCache.GetCourseOfFireDefinitionAsync( CourseOfFireDef );
        }

        /// <summary>
        /// Event Handler for when a SegmentGroupCommand is changed  (from within a <see cref="RangeScript"/>. This is used to update the DisableScoreProjection property based on the ResultEngineDirectives of the SegmentGroupCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SegmentGroupCommandChanged( object sender, EventArgs<SegmentGroupCommand> args ) {
            this.DisableScoreProjection = args?.Value?.GetResultEngineDirectives().DisableScoreProjection ?? false;
        }

        /// <summary>
        ///  Returns the <see cref="PaperTargetLabel"/> for this CourseOfFireStructure as specified by the PaperTargetLabelName property.
        ///  If no PaperTargetLabelName is specified, it returns the first PaperTargetLabel listed in the CourseOfFire definition.
        ///  If there are no PaperTargetLabels listed in the CourseOfFire definition, it returns a default PaperTargetLabel that
        ///  does not specify any barcodes to be printed.
        /// </summary>
        /// <returns></returns>
        public async Task<PaperTargetLabel> GetPaperTargetLabelAsync() {
            var cofDef = await GetCourseOfFireDefinitionAsync();

            // Try and find the PaperTargetLabel the user specified.
            if (!string.IsNullOrEmpty( PaperTargetLabelName )) {
                var label = cofDef.PaperTargetLabels.Find( x => x.PaperTargetLabelName == PaperTargetLabelName );
                if (label != null) {
                    return label;
                }
            }

            //If not found, return the first one in the list.
            if (cofDef.PaperTargetLabels.Count > 0) {
                return cofDef.PaperTargetLabels[0];
            }

            // Finally return the default NONE, as this COF likely is designed for ESTs, and therefore would not have a PaperTargetLabel value.
            return PaperTargetLabel.NONE;
        }
        #endregion
    }
}

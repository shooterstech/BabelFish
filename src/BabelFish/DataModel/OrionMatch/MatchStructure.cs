using System.Diagnostics;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataActors.ResultListMerger;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A MaytchStructure may either be used as the complete configuration of an existing <see cref="Match"/>
    /// or it may be used as a template to create a new Match.
    /// </summary>
    /// <remarks>New with BabelFish 2.0 / Orion 3.0 DataModel</remarks>
    public class MatchStructure :
        IMergedResultListContainer,
        IFinishInitializationAsync,
        G_STJ_SER.IJsonOnDeserializing,
        G_STJ_SER.IJsonOnDeserialized {

        #region Private and Protected Fields
        protected bool _ignoreEvents = false;
        #endregion

        #region Constructors, Facory Methods, and Initialization Methods
        /// <summary>
        /// Public constructor. Unless you are the deserializer, it is generally best to construct a new instance
        /// using the <see cref="MatchStructure(Match)"/> constructor, as this sets the container property <see cref="Match"/>.
        /// </summary>
        public MatchStructure() {

        }

        /// <summary>
        /// Preferred public constructor. Sets the container property <see cref="Match"/>.
        /// </summary>
        /// <param name="match">The Match that created and contains this instance.</param>
        public MatchStructure( Match match ) {
            this.Match = match;
        }

        /// <summary>
        /// Gets called after System.Text.Json deserializes an instance of this class.
        /// Sets _ignoreEvents to false so that events will fire as expected after deserialization.
        /// </summary>
        public void OnDeserialized() {
            //Populate the backwards pointer.
            foreach (var mrl in MergedResultLists) {
                mrl.Container = this;
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

        /// <inheritdoc/>
        public async Task FinishInitializationAsync() {
            foreach (var sharedAttribute in this.SharedAttributes) {
                await sharedAttribute.FinishInitializationAsync();
            }

            foreach (var cof in this.CoursesOfFire) {
                await cof.FinishInitializationAsync();
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Occurs when a new <see cref="CourseOfFireStructure"/> is added to this instance.
        /// <para>the preferred way of adding a new CourseOfFireStructure is to use the <see cref="AddCourseOfFireAsync(SetName)"/> method.</para>
        /// </summary>
        public event EventHandler<EventArgs<CourseOfFireStructure>> OnCourseOfFireAdded;

        /// <summary>
        /// Occurs when a new global <see cref="AttributeConfiguration"/> is added to this instance.
        /// <para>The preferred way of adding a new AttributeConfiguration is to use <see cref="AddAttributeConfigurationAsync(SetName)"/>. </para>
        /// </summary>
        public event EventHandler<EventArgs<AttributeConfiguration>> OnAttributeConfigurationAdded;

        public event EventHandler<EventArgs<MergedResultList>> OnMergedResultListAdded;
        #endregion

        #region Data Model Properties
        /// <summary>
        /// <para>Unless you are the deserializer, it is generally best to add new CourseOfFireStructures using the
        /// <see cref="AddCourseOfFireAsync(SetName)"/> method. As this method sets a known good value for CourseOfFireId.</para>
        /// </summary>
        public List<CourseOfFireStructure> CoursesOfFire { get; set; } = new List<CourseOfFireStructure>();

        /// <summary>
        /// All participants in a <see cref="Match"/> must have a value for each SharedAttributes, and that value is used within each COF.
        /// <para>Shared AttributeConfigurations are sometimes called global attributes, since all participants have one, and there AttributeValue
        /// is common accross all CoursesOfFire.</para>
        /// </summary>
        public List<AttributeConfiguration> SharedAttributes { get; set; } = new List<AttributeConfiguration>();

        /// <summary>
        /// A Match may have 0 or more MergedResultLists. Each MergedResultList describes a way to merge scores from different Courses of Fire's ResultLists together.
        /// </summary>
        public List<MergedResultList> MergedResultLists { get; set; } = new List<MergedResultList>();

        #endregion

        #region Helper Properties

        /// <summary>
        /// Pointer to the Match instance that contains this MatchStructure.
        /// </summary>
        [G_NS.JsonIgnore]
        public Match Match { get; internal set; }

        [G_NS.JsonIgnore]
        public MatchID MatchId {
            get {
                return Match.MatchID;
            }
        }

        [G_NS.JsonIgnore]
        public IResultListFetcher ResultListFetcher {
            get {
                return Match.Project;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Attempts to find the CourseOfFireStructure with courseOfFireId. Returns a boolean indicating it's success or lack there of.
        /// </summary>
        /// <param name="courseOfFireId"></param>
        /// <param name="cof"></param>
        /// <returns></returns>
        public bool TryGetCourseOfFireStructure( int courseOfFireId, out CourseOfFireStructure cof ) {
            cof = CoursesOfFire.Find( x => x.CourseOfFireId == courseOfFireId );

            return cof is not null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns>The newly added CoruseOfFire's Id.</returns>
        /// <exception cref="DefinitionNotFoundException">Thrown if the past in SetName is not a known COURSE OF FIRE definition.
        /// <exception cref="InvalidOperationException">Thrown if the Match already has 24 Courses of Fire.
        /// or it is the DEFUALT.</exception>
        public async Task<int> AddCourseOfFireAsync( SetName setName ) {
            // The max of 24 Courses of fire is somewhat arbitrary, but comes from keeping resources constrained and having 2 matches a month for a year.
            if (this.CoursesOfFire.Count >= 24) {
                throw new InvalidOperationException( "A Match cannot have more than 24 Courses of Fire." );
            }

            if (this.Match is null) {
                throw new InvalidOperationException( "The Match property of this CourseOfFireStructure is null. Likely means it was not set when this instance was created or deserialized." );
            }

            //NOTE this.Match.Project could be null if the Match was deserialized without its Project, such as when a Match is read from the REST API.

            var cof = await CourseOfFireStructure.CreateAsync( this, setName );

            var maxId = 0;
            foreach (var existingCof in CoursesOfFire) {
                if (maxId > existingCof.CourseOfFireId) {
                    maxId = existingCof.CourseOfFireId;
                }
            }
            cof.CourseOfFireId = maxId + 1;

            this.CoursesOfFire.Add( cof );

            if (!_ignoreEvents) {
                OnCourseOfFireAdded?.Invoke( this, new EventArgs<CourseOfFireStructure>( cof ) );
            }

            return cof.CourseOfFireId;
        }

        public async Task<AttributeConfiguration> AddAttributeConfigurationAsync( SetName attributeDef ) {

            if (attributeDef.IsDefault) {

                throw new ArgumentException( "May not add an AttributeConfiguration using default." );
            }

            var attributeConfig = await AttributeConfiguration.CreateAsync( attributeDef );
            attributeConfig.CourseOfFireId = 0; //0 indicates it is a shared attribute, and not specific to any one Course of Fire.
            SharedAttributes.Add( attributeConfig );

            if (Match is not null && Match.Project is not null) {
                foreach (var mp in Match.Project.Participants) {
                    if (attributeConfig.IsForIndividuals && mp.Participant.ParticipantType == ParticipantType.INDIVIDUAL) {
                        var avdp = await AttributeValueDataPacketMatch.CreateAsync( attributeConfig );
                        avdp.CourseOfFireId = 0; //0 indicates it is a shared attribute, and not specific to any one Course of Fire.
                        mp.Participant.AttributeValues.Add( avdp );
                    }
                }
            } else {
                Debug.Assert( Match is not null, "The Match property of this CourseOfFireStructure is null. Likely means it was not set when this instance was created or deserialized." );
                Debug.Assert( Match.Project is not null, "The Project property of the Match property of this CourseOfFireStructure is null. Likely means it was not set when this instance was created or deserialized." );
            }

            if (!_ignoreEvents) {
                OnAttributeConfigurationAdded?.Invoke( this, new EventArgs<AttributeConfiguration>( attributeConfig ) );
            }

            return attributeConfig;
        }

        /// <summary>
        /// Creates a new MergedResultList and adds it to <see cref="MergedResultLists"/>, then invokes the <see cref="OnMergedResultListAdded"/> event.
        /// The configuration of the MergedResultList is determined by the passed in MergeMethodType. For example, if MergeMethodType.SUM is passed in,
        /// then the MergedResultList's Configuration will be set to a new instance of SumMethodConfiguration.
        /// </summary>
        /// <param name="resultListName"></param>
        /// <param name="mergeMethodType"></param>
        /// <returns></returns>
        /// <remarks>NOTE This code is effectively the same as <see cref="Tournament.CreateMergedResultListAsync(MergeMethodType)"/>. If you change
        /// code here, change it there too.</remarks>
        public async Task<MergedResultList> CreateMergedResultListAsync( string resultListName, MergeMethodType mergeMethodType ) {

            var mrl = await MergedResultList.CreateAsync( this, resultListName, mergeMethodType );

            MergedResultLists.Add( mrl );

            if (!_ignoreEvents) {
                OnMergedResultListAdded?.Invoke( this, new EventArgs<MergedResultList>( mrl ) );
            }
            return mrl;
        }

        #endregion
    }
}

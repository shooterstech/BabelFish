using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A Match is the data model that describes a competition or a practice session within the sport of shooting.
    /// <para>The prefered method for deserializing from json is to use <see cref="LoadFromFileAsync(FileInfo)"/> or <see cref="LoadFromFileAsync(string)"/>.
    /// if you are deserializing outside of these methods besure to call FinishInitiializationAsync() before using your Match object.</para>
    /// </summary>
    [Serializable]
    public class Match : ISaveToFile, IFinishInitializationAsync {

        private Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Public constructor.
        /// </summary>
        public Match() { }


        /// <summary>
        /// After an object is deserialized form JSON,
        /// adds defaults to empty properties
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized()]
        public void OnDeserialized( StreamingContext context ) {
            if (ScoringSystems.Count == 0)
                ScoringSystems.Add( "Orion Scoring System" );
        }

        /// <summary>
        /// The name of the Match
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string Name {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Unique MatchID for the competition to get squadding for. Will match exactly (assuming no errors) of the MatchID in the GetMatchDetailRequest
        /// 
        /// This is a required field.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        public MatchID MatchID { get; set; } = MatchID.DEFAULT;

        /// <summary>
        /// The ParentID of this Match. Will vary from MatchID if the MatchID indicates this is a Child Virtual Match. Otherwise they are the same.
        /// </summary>
        /// <remarks>This property is not deprecated, however it will be removed from JSON serialization as its value may be derived from the MatchID.</remarks>
        [G_NS.JsonProperty( Order = 3 )]
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        [Obsolete( "Deprecated, because the value of ParentID can be derived from the MatchID. Deprecated Match 2024." )]
        public MatchID? ParentID {
            get {
                return this.MatchID.GetParentMatchID();
            }
        }

        /// <summary>
        /// Returns the .ParentID as a MatchID instance.
        /// </summary>
        [Obsolete( "Use .ParentID" )]
        public MatchID GetParentId() {
            return ParentID;
        }

        /// <summary>
        /// Returns the start date of the match. Which is defined as the earliest <see cref="CourseOfFireStructure.StartDate">Start Date</see>
        /// amongst the defined <see cref="CourseOfFireStructure" /> within <see cref="CourseOfFireStructure"/>. If no CourseOfFireStructure
        /// is yet defined, then Today is returned.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate {
            get {
                if (this.MatchStructure.CoursesOfFire.Count == 0)
                    return DateTime.Today;

                var startDate = DateTime.MaxValue;
                foreach (var cof in this.MatchStructure.CoursesOfFire) {
                    if (cof.StartDate < startDate) {
                        startDate = cof.StartDate;
                    }
                }

                return startDate;
            }
        }


        /// <summary>
        /// Returns the end date of the match. Which is defined as the latest <see cref="CourseOfFireStructure.EndDate">End Date</see>
        /// amongst the defined <see cref="CourseOfFireStructure" /> within <see cref="CourseOfFireStructure"/>. If no CourseOfFireStructure
        /// is yet defined, then Today is returned.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime EndDate {
            get {
                if (this.MatchStructure.CoursesOfFire.Count == 0)
                    return DateTime.Today;

                var endDate = DateTime.MinValue;
                foreach (var cof in this.MatchStructure.CoursesOfFire) {
                    if (cof.EndDate > endDate) {
                        endDate = cof.EndDate;
                    }
                }

                return endDate;
            }
        }

        /// <summary>
        /// External Result URL
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public string ResultURL { get; set; } = string.Empty;

        /// <summary>
        /// Sets the public visibility for the match. Valid values are
        /// Private : No visibility
        /// Protected : Only athletes, their coaches, and match officials may see
        /// Club : Same as participant, but also includes all club members of the sponsoring club
        /// Public : Everyone may view
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /// <summary>
        /// The type of match, as specified by the Match Director.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        public CompetitionTypeOptions MatchType { get; set; } = CompetitionTypeOptions.LOCAL_MATCH;

        /// <summary>
        /// The location of the match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 20 )]
        [G_NS.JsonProperty( Order = 20 )]
        public Location Location { get; set; } = new Location();

        /// <summary>
        /// Contact information for the match administrators.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
        public List<Contact> MatchContacts { get; set; } = new List<Contact>();


        #region BabelFish 2.0 / Orion 3.0 DataModel
        /*
         * The properties in this region are new with the release of BabelFish 2.0 and Orion version 3.0, 
         * and are not populated in older versions of the software. They are largely related to the new 
         * feature of supporting multiple Courses of Fire within a Match, and the new Match Structure that supports that feature. 
         */
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>New with BabelFish 2.0 / Orion 3.0 DataModel</remarks>
        [G_STJ_SER.JsonPropertyOrder( 30 )]
        [G_NS.JsonProperty( Order = 30 )]
        public MatchStructure MatchStructure { get; set; } = new MatchStructure();
        #endregion

        /// <summary>
        /// The names of the scoring system used to score shots within this match. For example, "Orion Scoring System."
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 40 )]
        [G_NS.JsonProperty( Order = 40 )]
        public List<string> ScoringSystems { get; set; } = new List<string>();

        /// <summary>
        /// Newtonsoft.json helper method, to determine if ScoreSystems property should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeScoringSystems() {
            return ScoringSystems != null && ScoringSystems.Count > 0;
        }

        /// <summary>
        /// The type of scoring system used in this match, such as PAPER_TARGER or EST.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 41 )]
        [G_NS.JsonProperty( Order = 41 )]
        [DefaultValue( ScoringSystem.UNKNOWN )]
        public ScoringSystem ScoringSystemType { get; set; } = ScoringSystem.UNKNOWN;

        #region Move To Seperate Objects
        /*
         * The properties in this region are important as part of the REST API return Get Match Detail(),
         * however they do not belong as part of the Match object itsefl. Think they should be moved to a 
         * new object that is returned as part of the Get Match Detail() API call.
         */

        /// <summary>
        /// A list of MatchHtmlReports (e.g. pressrelease.html) that exist for this match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 50 )]
        [G_NS.JsonProperty( Order = 50 )]
        public List<MatchHtmlReport> HtmlReports { get; set; } = new List<MatchHtmlReport>();

        /// <summary>
        /// Newtonsoft.json helper method, to determine if HtmlReports property should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeHtmlReports() {
            return HtmlReports is not null && HtmlReports.Count > 0;
        }

        /// <summary>
        /// A list of authorized capabilities the caller has for this match. These values are 
        /// returned by the Rest API, but are not serialized. Instead 'AuthorizationList'
        /// is sent, and the list of Authorizations is derved using it and the caller's identificaiton.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 51 )]
        [G_NS.JsonProperty( Order = 51 )]
        public List<MatchAuthorizationCapability> Authorization { get; set; } = new List<MatchAuthorizationCapability>();

        /// <summary>
        /// NewtonSoft helper method to determine if Authoirazation should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeAuthorization() {
            return Authorization is not null && Authorization.Count > 0;
        }
        #endregion

        /// <summary>
        /// The orion account or at home account who owns this match.
        /// </summary>
        /// <example>OrionAcct000001 or AtHomeAcct123456</example>
        [G_STJ_SER.JsonPropertyOrder( 96 )]
        [G_NS.JsonProperty( Order = 96 )]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software that created or last updated this match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 97 )]
        [G_NS.JsonProperty( Order = 97 )]
        public string Creator { get; set; }

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

        #region Deprecated Properties
        /*
         * The properties in this region are deprecated, and should not be used anymore. They are only kept here for backward compatibility with older versions of the API, and to avoid breaking changes. They will eventually be removed in a future version, but for now they are marked as Obsolete.
         */

        /// <summary>
        /// The list of Events in the Match that have Result Lists associated with them.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 60 )]
        [G_NS.JsonProperty( Order = 60 )]
        [Obsolete( "Starting with BabelFish 2.0 (Orion version 3.0), Matches will be able to have multiple COURSES OF FIRE, " +
            "and each CourseOfFireDef will have its own list of Results. The multiple Courses of Fire are defined within the MatchStructure property" +
            "Deprecated March 2026." )]
        public List<ResultEventAbbr> ResultEvents { get; set; } = new List<ResultEventAbbr>();

        /// <summary>
        /// The COURSE OF FIRE definition used in the conduct of this match.
        /// </summary>
        /// <remarks>In the future MatchV2 class, Matches will be able to have multiple COURSES OF FIRE. This property will be replaced.</remarks>
        [G_STJ_SER.JsonPropertyOrder( 61 )]
        [G_NS.JsonProperty( Order = 61 )]
        [Obsolete( "Starting with BabelFish 2.0 (Orion version 3.0), Matches will be able to have multiple COURSES OF FIRE, " +
            "and each CourseOfFireDef will have its own CourseOfFireDef. The multiple Courses of Fire are defined within the MatchStructure property." +
            "Deprecated Martch 2026" )]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// SetName of the ScoreConfig used in this match.
        /// NOTE: The name of the ScoreFormatCollection is specified in the Course of Fire 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 62 )]
        [G_NS.JsonProperty( Order = 62 )]
        [Obsolete( "Starting with BabelFish 2.0 (Orion version 3.0), Matches will be able to have multiple COURSES OF FIRE, " +
            "and each CourseOfFireDef will have its own ScoreConfigName. The multiple Courses of Fire are defined within the MatchStructure property" +
            "Deprecated March 2026" )]
        public string ScoreConfigName { get; set; }

        /// <summary>
        /// Name of the TargetCollection used in this match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 63 )]
        [G_NS.JsonProperty( Order = 63 )]
        [Obsolete( "Starting with BabelFish 2.0 (Orion version 3.0), Matches will be able to have multiple COURSES OF FIRE, " +
            "and each CourseOfFireDef will have its own TargetCollectionName. The multiple Courses of Fire are defined within the MatchStructure property" +
            "Deprecated Martch 2026" )]
        public string TargetCollectionName { get; set; }

        /// <summary>
        /// The list of Events in the Match that have squadding. Also contains definitions on the types of OrionTargets used.
        /// 
        /// To pull the full squadding, use GetSquaddingListRequest()
        /// </summary>
        [G_NS.JsonProperty( Order = 64 )]
        [G_STJ_SER.JsonPropertyOrder( 64 )]
        [Obsolete( "Starting with BabelFish 2.0 (Orion version 3.0), Matches will be able to have multiple COURSES OF FIRE, " +
            "and each CourseOfFireDef will have its own Squadding. The multiple Courses of Fire are defined within the MatchStructure property" +
            "Deprecated March 2026" )]
        public List<SquaddingEvent> SquaddingEvents { get; set; } = new List<SquaddingEvent>();

        /// <summary>
        /// The UTC time that the ResultEvent (scores for the match) were last updated.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 65 )]
        [G_NS.JsonProperty( Order = 65 )]
        [Obsolete( "Property is not really used anymore. Deprecated March 2026" )]
        public DateTime ResultEventsLastUpdate { get; set; } = new DateTime();

        /// <summary>
        /// List of Attribute SetNames used in this match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 66 )]
        [G_NS.JsonProperty( Order = 66 )]
        [Obsolete( "Starting with BabelFish 2.0 (Orion version 3.0), Matches will be able to have multiple COURSES OF FIRE, " +
            "and each CourseOfFireDef will have its set of Attributes. The multiple Courses of Fire are defined within the MatchStructure property." +
            "Deprecated March 2026." )]
        public List<string> AttributeNames { get; set; } = new List<string>();

        /// <summary>
        /// The SharedKey is a defacto password. Allowing systems on the outside to
        /// make change requests to the match, such as add athletes or teams, insert
        /// shot data, etc.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 70 )]
        [G_NS.JsonProperty( Order = 70 )]
        [Obsolete( "Replaced with user based Authorization with RoleAuthorization." +
            "Deprecated December 2025." )]
        public string SharedKey { get; set; } = String.Empty;

        #endregion

        /*
         * EKA Note November 2025
         * Removed as a property, because Role Authorization is saved instead to the MatchParticipant object. No need to replicate that data here.
         *
        /// <summary>
        /// A list of Authorization roles participants in the match have.
        /// This list is sent to the Cloud, but is never seen as part of the Rest API. Instead
        /// the Rest API sends back a list of Authorizations the caller has in the match, with 
        /// the Property 'Authorization'.
        ///
        /// This list is only ever uploaded to the cloud. It is never (or at least should never) be
        /// sent back as part of an API request. 
        /// </summary>G_STJ_SER.G_STJ_SER.JsonPropertyOrder
        public List<MatchAuthorization> AuthorizationList { get; set; } = new List<MatchAuthorization>();
        */

        /*
         * EKA Note November 2025
         * Removed as a property, because its been marked as deprecated for so long.
         * 
        /// <summary>
        /// A list of match participants, but only the athletes, not the teams. 
        /// 
        /// This information is largely rhetotical with Get Participant List API call.
        ///
        /// This list is only ever uploaded to the cloud. It is never (or at least should never) be
        /// sent back as part of an API request.
        /// </summary>
        [Obsolete( "Will be replaced soon with a more proper participant list." )]
        public List<MatchParticipantResult> MatchParticipantResults { get; set; } = new List<MatchParticipantResult>();
        */

        /*
         * EKA Note November 2025
         * Removed as a property, because its been marked as deprecated for so long.
         * 
        /// <summary>
        /// A list of Result COF that the logged in user owns for this match. Meaning, these are the
        /// scores the logged in user shot. If a user is not logged in, or the logged in user is
        /// not an athletes, then this will be an empty list.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder ( 20 )]
        [Obsolete( "Format of this data is in the old ResultCOF (pre 2022). Make a separate call using GetResultCOF() instead, which returns data in the 2022 format." )]
        public List<ResultCOF> ParticipantResults { get; set; } = new List<ResultCOF>();
        */

        /// <summary>
        /// Helper function that indicates if this Match is currently going on. Which is 
        /// determined by the Match's Start and End Date.
        /// </summary>
        [G_NS.JsonIgnore]
        public bool IsOnGoing {
            get {
                return StartDate <= DateTime.Today
                    && EndDate >= DateTime.Today;
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "MatchDetail for " );
            foo.Append( Name );
            return foo.ToString();
        }

        /// <inheritdoc/>
        public string GetFileName() {
            return $"{Name}.json";
        }

        /// <summary>
        /// SAves the current Match object to a JSON file in the specified relative directory.
        /// The file name is derived from the Match's Name property.
        /// </summary>
        /// <param name="relativeDirectory">Usually the My Matches directory.</param>
        /// <returns>The full path to the saved file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string SaveToFile( DirectoryInfo relativeDirectory ) {

            if (relativeDirectory == null)
                throw new ArgumentNullException( nameof( relativeDirectory ) );

            string filePath = Path.Combine( relativeDirectory.FullName, GetRelativePath() );

            var directoryPath = Path.GetDirectoryName( filePath );

            if (!Directory.Exists( directoryPath )) {
                Directory.CreateDirectory( directoryPath );
            }

            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            File.WriteAllText( filePath, json );

            return filePath;
        }

        /// <summary>
        /// Serializes the current object to JSON format and writes it to the specified file.
        /// </summary>
        /// <remarks>This method uses Newtonsoft.Json for serialization and overwrites the file if it
        /// already exists.</remarks>
        /// <param name="fileInfo">The file information representing the target file where the JSON data will be saved. Cannot be null.</param>
        /// <returns>The full path of the file where the JSON data has been written.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fileInfo"/> is null.</exception>
        public string SaveToFile( FileInfo fileInfo ) {

            if (fileInfo == null)
                throw new ArgumentNullException( nameof( fileInfo ) );

            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            File.WriteAllText( fileInfo.FullName, json );

            return fileInfo.FullName;
        }

        /// <summary>
        /// Returns the standard relative path for this Match. It is relative to the My Matches directory.
        /// </summary>
        /// <returns></returns>
        public string GetRelativePath() {
            return Path.Combine( this.Name, this.GetFileName() );
        }

        /// <summary>
        /// Serializes the current Match object to a JSON string using the BabelFish's standard
        /// Newtonsoft.Json's serialization options.
        /// </summary>
        /// <returns>A JSON string that represents the current object, formatted according to the configured Newtonsoft.Json
        /// serializer settings.</returns>
        public string SerializeToJson() {
            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            return json;
        }

        /// <summary>
        /// Reads a Match object from a JSON file. The JSON file is expected to be formatted according to the BabelFish's standard.
        /// <para>The method automatically calls the <see cref="FinishInitializationAsync"/> method after deserialization to ensure that all AttributeValues are fully initialized.</para>
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<Match> LoadFromFileAsync( FileInfo fileInfo ) {

            if (fileInfo == null)
                throw new ArgumentNullException( nameof( fileInfo ) );
            using (var stream = fileInfo.OpenRead()) {
                var match = G_STJ.JsonSerializer.Deserialize<Match>( stream, Helpers.SerializerOptions.SystemTextJsonDeserializer );
                await match.FinishInitializationAsync();
                return match;
            }
        }

        public static async Task<Match> LoadFromFileAsync( string fullPath ) {

            FileInfo fileInfo = new FileInfo( fullPath );
            return await LoadFromFileAsync( fileInfo );
        }

        /// <inheritdoc />
        /// <remarks>The prefered method for deserializing from json is to use <see cref="LoadFromFileAsync(FileInfo)"/> or <see cref="LoadFromFileAsync(string)"/>.
        /// if you are deserializing outside of these methods besure to call FinishInitiializationAsync() before using your Match object.</remarks>
        public async Task FinishInitializationAsync() {
            await this.MatchStructure.FinishInitializationAsync();
        }
    }
}

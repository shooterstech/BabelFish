using System.ComponentModel;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A MatchParticipant is a <see cref="Individual"/> or a <see cref="Team"/> participating in a <see cref="Match"/>.
    /// </summary>
    [Serializable]
    public class MatchParticipant :
        IParticipant,
        ISaveToFile,
        IFinishInitializationAsync,
        G_STJ_SER.IJsonOnDeserializing,
        G_STJ_SER.IJsonOnDeserialized {

        /// <summary>
        /// The Folder name, with respect to the MatchProject's root directory, that MatchParticipant instances are stored in.
        /// </summary>
        public const string FOLDER_NAME = "Participants";

        #region Private Variables
        private bool _ignoreEvents = false;

        #endregion

        #region Constructors, Facory Methods, and Initialization Methods
        /// <summary>
        /// Default public constructor. Unless you are a deserializer, it is best to use the alternative constructor that takes a
        /// ParticipantType, so that the Participant property is initialized to the correct concrete class, either <see cref="Individual"/> or <see cref="Team"/>. 
        /// </summary>
        public MatchParticipant() {
            Participant = new Individual();
            Participant.MatchParticipant = this;
        }

        public MatchParticipant( MatchProject project, ParticipantType participantType ) {
            this.Project = project;

            if (participantType == ParticipantType.INDIVIDUAL) {
                Participant = new Individual();
            } else {
                Participant = new Team();
            }

            Participant.MatchParticipant = this;
            this.Participant.OnDisplayNameChanged += RenameFile;
        }

        /// <summary>
        /// Reads a Match object from a JSON file. The JSON file is expected to be formatted according to the BabelFish's standard.
        /// <para>The method automatically calls the <see cref="FinishInitializationAsync"/> method after deserialization to ensure that all AttributeValues are fully initialized.</para>
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<MatchParticipant> LoadFromFileAsync( FileInfo fileInfo ) {

            if (fileInfo == null)
                throw new ArgumentNullException( nameof( fileInfo ) );
            using (var stream = fileInfo.OpenRead()) {
                var mp = G_STJ.JsonSerializer.Deserialize<MatchParticipant>( stream, Helpers.SerializerOptions.SystemTextJsonDeserializer );
                await mp.FinishInitializationAsync();
                return mp;
            }
        }

        /// <summary>
        /// Reads a Match object from a JSON file. The JSON file is expected to be formatted according to the BabelFish's standard.
        /// <para>The method automatically calls the <see cref="FinishInitializationAsync"/> method after deserialization to ensure that all AttributeValues are fully initialized.</para>
        /// </summary>
        /// <param name="fullPath">The full path to the file from which to load the match. This path must be valid and accessible.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the loaded match.</returns>
        public static async Task<MatchParticipant> LoadFromFileAsync( string fullPath ) {

            FileInfo fileInfo = new FileInfo( fullPath );
            return await LoadFromFileAsync( fileInfo );
        }

        public void OnDeserialized() {
            foreach (var entry in Entries) {
                entry.MatchParticipant = this;
            }

            this.Participant.MatchParticipant = this;

            _ignoreEvents = false;
        }

        public void OnDeserializing() {
            _ignoreEvents = true;
        }
        #endregion

        #region Data Model Properties
        /// <summary>
        /// Gets or sets the name of the match that this Participant participanted in.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string MatchName { get; set; } = string.Empty;

        /// <summary>
        /// The unique Match ID that this Participant participanted in.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public MatchID MatchID { get; set; } = MatchID.DEFAULT;

        /// <summary>
        /// Unique ID within this match, for this Match Participant.
        /// <para>ParticipantID differs from a Competitor Number in two ways. First, a Team does not have competitor numbers. Second, once created ParticipantId may not be changed while competitor numbers can.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string ParticipantID { get; set; } = Scopos.BabelFish.Helpers.Common.GenerateUniqueId();

        /// <summary>
        /// UUID formatted Scopos Account user id.
        /// <para>If missing or an empty string, likely means this Participant is either a Team, or is an Individual but does not have a Scopos Account.</para>
        /// </summary>
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 4 )]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// Information about the Participant, which can be either an Individual or a Team. Includes name, attribute values, and if a team, the list of team members.
        /// <para>It is recomended NOT to change this value. If you need to change the Participant, create a whole new instance of MatchParticipant instead.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public Participant Participant { get; set; }

        /// <summary>
        /// A list of entries (CourseOfFireEntry) for this Participant. Basically say which events this Participant is entered in,
        /// and what their squadding assignment is for each event.
        /// <para>The preferred method for creating a new entry is to use the <see cref="CreateEntry(int)"/> method.</para>
        /// </summary>
        /// <remarks>This property replaced MatchParticipantResults</remarks>
        [G_NS.JsonProperty( Order = 6 )]
        public List<CourseOfFireEntry> Entries { get; set; } = new List<CourseOfFireEntry>();


        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software.
        /// </summary>
        [G_NS.JsonProperty( Order = 97 )]
        public string Creator { get; set; } = string.Empty;

        /// <summary>
        /// When serialized, this is the BableFish version string that the data model of this MatchParticipant instance adheres to. 
        /// </summary>
        /// <remarks>Since each MatchParticipant is serialized seperatly, each instnace needs to store the JSON Version. In practice though
        /// all MatchParticipant instances within the same MatchProject and ParticipantList will have the same JSON Version.</remarks>
        [G_STJ_SER.JsonPropertyOrder( 98 )]
        [G_NS.JsonProperty( Order = 98 )]
        public string JSONVersion { get; set; } = Helpers.Common.DATA_MODEL_VERSION;

        /// <summary>
        /// UTC Time that this MatchParticipant was last updated. Thsi value is not set automatically, but should be set by the caller when changes are made to this MatchParticipant that should be tracked.
        /// This is intended to be used for synchronization and conflict resolution when multiple users are editing the same Match data.
        /// </summary>
        [G_NS.JsonProperty( Order = 99 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        #endregion

        #region Helper Properties
        /// <summary>
        /// Helper property to indicate if this MatchParticipant is an Individual or a Team, based on the ParticipantType of the Participant property.
        /// </summary>
        [G_NS.JsonIgnore]
        public bool IsTeam {
            get {
                return Participant.ParticipantType == ParticipantType.TEAM;
            }
        }

        /// <summary>
        /// Backwards pointer to the project holding this MatchParticipant. 
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchProject? Project { get; internal set; } = null;
        #endregion

        #region Methods
        public CourseOfFireEntry CreateEntry( int courseOfFireId ) {
            int currentEntryIndex = Entries.FindIndex( e => e.CourseOfFireId == courseOfFireId );
            if (currentEntryIndex == -1) {

                CourseOfFireEntry entry;
                if (this.Participant.ParticipantType == ParticipantType.INDIVIDUAL) {
                    entry = new CourseOfFireEntryIndividual();
                    entry.EntryStatus = EntryStatus.NOT_ENTERED;
                } else {
                    entry = new CourseOfFireEntryTeam();
                }

                entry.CourseOfFireId = courseOfFireId;
                entry.MatchParticipant = this;
                Entries.Add( entry );
                return entry;
            } else {
                return Entries[currentEntryIndex];
            }
        }

        /// <summary>
        /// Attempts to locate and return an existing CourseOfFireEntry in the Entries list with the specified courseOfFireId. Returns true if an entry with the specified courseOfFireId is found, and false otherwise.
        /// </summary>
        /// <param name="courseOfFireId"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool TryGetEntryByCourseOfFireId( int courseOfFireId, out CourseOfFireEntry entry ) {
            int currentEntryIndex = Entries.FindIndex( e => e.CourseOfFireId == courseOfFireId );
            if (currentEntryIndex == -1) {
                entry = null;
                return false;
            } else {
                entry = Entries[currentEntryIndex];
                return true;
            }
        }

        /// <summary>
        /// Attempts to locate and return an existing CourseOfFireEntry in the Entries list with the specified courseOfFireId.
        /// If an entry with the specified courseOfFireId is found, it is returned. If not, a new CourseOfFireEntry is created w
        /// ith the specified courseOfFireId, added to the Entries list, and returned.
        /// </summary>
        /// <param name="courseOfFireId"></param>
        /// <returns></returns>
        public CourseOfFireEntry GetEntryByCourseOfFireId( int courseOfFireId ) {
            int currentEntryIndex = Entries.FindIndex( e => e.CourseOfFireId == courseOfFireId );
            if (currentEntryIndex > 0) {
                return Entries[currentEntryIndex];
            } else {
                return CreateEntry( courseOfFireId );
            }
        }

        /// <inheritdoc/>
        public override string ToString() {
            return Participant.DisplayName;
        }
        #endregion

        #region ISaveToFile Implementation
        /// <inheritdoc/>
        /// <param name="composite">Expected to be a <see cref="MatchProject"/> instance.</param>
        public string GetFileName() {
            return $"{StringFormatting.MakeSafeFileName( Participant.DisplayName )} {this.ParticipantID}.json";
        }

        /// <summary>Gets the relative path, with respect to the MatchProject's path, for the file associated with this MatchParticipant.</summary>
        public string GetRelativePath() {
            // MatchParticipant instances are stored directly into the MatchProject folder, so the relative path is just the file name

            return Path.Combine( FOLDER_NAME, this.GetFileName() );
        }

        public string SaveToFile() {
            if (Project == null)
                throw new InvalidOperationException( "Cannot save MatchParticipant to file because its Project property is not set." );

            return SaveToFile( Project.ProjectDirectory );
        }

        /// <inheritdoc/>
        /// <param name="matchProjectDirectory">The directory to save this instance to. Generally it will be the MatchProject's root directory <see cref="MatchProject.ProjectDirectory"/></param>
        /// <exception cref="InvalidOperationException">Thrown if the MatchProject property Project is not set.</exception>
        public string SaveToFile( DirectoryInfo matchProjectDirectory ) {

            if (matchProjectDirectory == null)
                throw new ArgumentNullException( nameof( matchProjectDirectory ) );

            string filePath = Path.Combine( matchProjectDirectory.FullName, GetRelativePath() );

            var directoryPath = Path.GetDirectoryName( filePath );

            if (!Directory.Exists( directoryPath )) {
                Directory.CreateDirectory( directoryPath );
            }

            string json = SerializeToJson();

            File.WriteAllText( filePath, json );

            return filePath;
        }

        /// <inheritdoc/>
        public string SaveToFile( FileInfo fileInfo ) {

            if (fileInfo == null)
                throw new ArgumentNullException( nameof( fileInfo ) );

            string json = SerializeToJson();

            File.WriteAllText( fileInfo.FullName, json );

            return fileInfo.FullName;
        }

        /// <inheritdoc/>
        public string SerializeToJson() {
            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            return json;
        }

        private void RenameFile( object sender, EventArgs<Participant> e ) {
            /*
             * If the Participant's Display Name changes, we need to rename the file to match the new display name. 
             * This is because the file name is based on the Display Name, and we want to keep it consistent.
             * However, since Display Name is not guaranteed to be unique, we are currently using ParticipantID for the file name instead, so this method is not needed. 
             * If we switch back to using Display Name for the file name in the future, we will need to implement this method.
             */

            if (!string.IsNullOrEmpty( Participant.PreviousDisplayName )) {
                var previousFileName = $"{StringFormatting.MakeSafeFileName( Participant.PreviousDisplayName )} {this.ParticipantID}.json";
                var newFileName = this.GetFileName();

                string oldPath = Path.Combine( Project.ProjectDirectory.FullName, FOLDER_NAME, previousFileName );
                string newPath = Path.Combine( Project.ProjectDirectory.FullName, FOLDER_NAME, newFileName );

                if (File.Exists( oldPath )) {
                    File.Move( oldPath, newPath );
                }

                this.SaveToFile();
            }
        }
        #endregion

        #region IFinishInitializationAsync Implementation

        /// <inheritdoc />
        /// <remarks>The prefered method for deserializing from json is to use <see cref="LoadFromFileAsync(FileInfo)"/> or <see cref="LoadFromFileAsync(string)"/>.
        /// if you are deserializing outside of these methods besure to call FinishInitiializationAsync() before using your MatchParticipant object.</remarks>
        public async Task FinishInitializationAsync() {
            await this.Participant.FinishInitializationAsync();
        }

        #endregion
    }
}

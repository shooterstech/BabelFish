using System.Collections.Concurrent;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class MatchProject :
        ISaveToFile,
        IResultListFetcher,
        G_STJ_SER.IJsonOnDeserialized,
        G_STJ_SER.IJsonOnDeserializing {

        #region Private and Protected Fields

        private bool _ignoreEvents = false;
        private string _projectName;

        private ConcurrentDictionary<string, MatchParticipant> _participantsByResultCOFID = new ConcurrentDictionary<string, MatchParticipant>();
        #endregion

        #region Constructors, Facory Methods, and Initialization Methods
        public MatchProject() {

            this.OnProjectNameChanged += RenameFile;
        }

        public static async Task<MatchProject> CreateAsync( ClubAbbr club, string matchName, DirectoryInfo myMatchesDirectory ) {
            if (matchName.Length < 10)
                throw new ArgumentException( "MatchName must be at least 10 characters long to ensure valid file names and paths." );

            var project = new MatchProject();
            project.Match = new Match( club, project );
            project.ProjectName = matchName;
            project.Match.Name = matchName;

            project.ProjectDirectory = new DirectoryInfo( Path.Combine( myMatchesDirectory.FullName, project.ProjectName ) );
            project.ProjectDirectory.Create();

            project.ShotMapper = new ShotMapper( project );

            return project;
        }

        public static async Task<MatchProject> LoadFromFileAsync( FileInfo fileInfo ) {

            if (fileInfo == null)
                throw new ArgumentNullException( nameof( fileInfo ) );

            using (var stream = fileInfo.OpenRead()) {
                var matchProject = G_STJ.JsonSerializer.Deserialize<MatchProject>( stream, Helpers.SerializerOptions.SystemTextJsonDeserializer );
                matchProject.ProjectDirectory = fileInfo.Directory;

                //Load the Match.
                matchProject.Match = await Match.LoadFromFileAsync( Path.Combine( matchProject.ProjectDirectory.FullName, matchProject.MatchFileName ) );

                //Load the Participants
                var participantDirectory = new DirectoryInfo( Path.Combine( matchProject.ProjectDirectory.FullName, MatchParticipant.FOLDER_NAME ) );
                if (participantDirectory.Exists) {
                    foreach (var participantFile in participantDirectory.GetFiles()) {
                        var participant = await MatchParticipant.LoadFromFileAsync( participantFile );
                        matchProject.Participants.Add( participant );
                    }
                }

                return matchProject;
            }
        }

        public static async Task<MatchProject> LoadFromFileAsync( string fullPath ) {

            FileInfo fileInfo = new FileInfo( fullPath );
            return await LoadFromFileAsync( fileInfo );
        }

        /// <summary>
        /// This method is called after deserialization with System.Text.Json. 
        /// </summary>
        public void OnDeserialized() {
            _ignoreEvents = false;
        }

        /// <summary>
        /// Method is called before deserialization with System.Text.Json.
        /// </summary>
        public void OnDeserializing() {
            _ignoreEvents = true;
        }
        #endregion

        #region Event Handlers
        [G_NS.JsonIgnore]
        EventHandler<EventArgs<MatchProject>> OnProjectNameChanged;
        #endregion

        #region Data Model Properties

        /// <summary>
        /// Gets the name of this MatchProject, which is derived from the name of the Match. This is used to create file and directory names,
        /// so it is made safe for that purpose by removing or replacing characters that are not allowed in file names.
        /// <para>Do not update it's value after the MatchProject is created.</para>
        /// </summary>
        public string ProjectName {
            get { return _projectName; }
            set {
                var safeValue = StringFormatting.MakeSafeFileName( value );
                if (_projectName != safeValue) {
                    PreviousProjectName = _projectName;
                    _projectName = safeValue;

                    if (!_ignoreEvents) {
                        OnProjectNameChanged?.Invoke( this, new EventArgs<MatchProject>( this ) );
                    }
                }
            }
        }

        public string MatchFileName { get; set; }

        [G_NS.JsonIgnore]
        public Match Match { get; private set; }

        /*
         * EKA March 2026: Isn't this the same as a MatchParticipantList? Do we need both? Maybe we can just use a MatchParticipantList here instead of a List<MatchParticipant>?
         */
        [G_NS.JsonIgnore]
        public List<MatchParticipant> Participants { get; set; } = new List<MatchParticipant>();

        #endregion

        #region Helper Properteis

        [G_NS.JsonIgnore]
        public string PreviousProjectName { get; private set; }

        [G_NS.JsonIgnore]
        public DirectoryInfo ProjectDirectory { get; set; }

        [G_NS.JsonIgnore]
        public ShotMapper ShotMapper { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="Individual"/> participant for the match. Adding a <see cref="CourseOfFireEntryIndividual"> for that individual
        /// to all Courses of Fire in the match, and adding AttributeValues for all SharedAttributes and CourseOfFire-specific attributes
        /// defined in the match structure.
        /// </summary>
        /// <param name="familyName">The family name of the individual.</param>
        /// <param name="givenName">The given name of the individual.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="MatchParticipant"/>.</returns>
        public async Task<MatchParticipant> CreateMatchParticipantAsync( string familyName, string givenName ) {

            MatchParticipant mp = new MatchParticipant( this, ParticipantType.INDIVIDUAL );
            mp.Project = this;
            mp.MatchID = Match.MatchID;
            mp.MatchName = Match.Name;

            var individual = (Individual)mp.Participant;
            individual.FamilyName = familyName;
            individual.GivenName = givenName;

            Participants.Add( mp );

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                var entry = (CourseOfFireEntryIndividual)mp.CreateEntry( cof.CourseOfFireId );
                _participantsByResultCOFID.TryAdd( entry.ResultCofId, mp );
            }

            foreach (var attributeConfiguration in Match.MatchStructure.GlobalAttributes) {
                if (!attributeConfiguration.Constant && attributeConfiguration.IsForIndividuals) {
                    individual.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ) );
                }
            }

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                foreach (var attributeConfiguration in cof.Attributes) {
                    if (!attributeConfiguration.Constant && attributeConfiguration.IsForTeams) {
                        individual.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ) );
                    }
                }
            }

            return mp;
        }

        /// <summary>
        /// Creates a new <see cref="Team"/> participant for the match. Adding a <see cref="CourseOfFireEntryTeam"/> for that team.
        /// </summary>
        /// <param name="teamName">The name of the team.</param>
        /// <returns>Returns the created <see cref="MatchParticipant"/>.</returns>
        public async Task<MatchParticipant> CreateMatchParticipantAsync( string teamName ) {

            MatchParticipant mp = new MatchParticipant( this, ParticipantType.TEAM );
            mp.Project = this;
            mp.MatchID = Match.MatchID;
            mp.MatchName = Match.Name;

            var team = (Team)mp.Participant;
            team.TeamName = teamName;
            Participants.Add( mp );

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                var entry = mp.CreateEntry( cof.CourseOfFireId );
                //No need to save Result COF ID to _participantsByResultCOFID here, because teams don't have Result COF IDs. Only individuals do.
            }

            foreach (var attributeConfiguration in Match.MatchStructure.GlobalAttributes) {
                if (!attributeConfiguration.Constant && attributeConfiguration.IsForTeams) {
                    team.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ) );
                }
            }

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                foreach (var attributeConfiguration in cof.Attributes) {
                    if (!attributeConfiguration.Constant && attributeConfiguration.IsForTeams) {
                        team.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ) );
                    }
                }
            }

            return mp;
        }

        /// <summary>
        /// Tries and finds the <see cref="MatchParticipant"/> associated with the specified result COFID. 
        /// </summary>
        /// <param name="resultCOFID">UUID formatted Result COF ID.</param>
        /// <param name="participant">If a match was made, this will return the MatchParticipant associated with the specified result COFID.</param>
        /// <returns>A boolean indicating whether the participant was found.</returns>
        public bool TryGetParticipantByResultCOFID( string resultCOFID, out MatchParticipant participant ) {
            return _participantsByResultCOFID.TryGetValue( resultCOFID, out participant );
        }

        /// <summary>
        /// Tries and finds the <see cref="CourseOfFireEntryIndividual"/> associated with the specified result COFID..
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool TryGetCourseOfFireEntryByResultCOFID( string resultCOFID, out CourseOfFireEntryIndividual? entry ) {
            if (_participantsByResultCOFID.TryGetValue( resultCOFID, out MatchParticipant participant )) {
                foreach (var entryToTest in participant.Entries) {
                    if (entryToTest is CourseOfFireEntryIndividual invEntry
                        && invEntry.ResultCofId == resultCOFID) {
                        entry = invEntry;
                        return true;
                    }
                }
            }

            entry = null;
            return false;
        }

        /// <inheritdoc />
        public Task<List<ResultList>> GetResultListsAsync( MergedResultList mergedResultList ) => throw new NotImplementedException();

        public void SetScoringSystem( ScoringSystem scoringSystemType, string nameOfScoringSystem ) {
#if DEBUG
            // No-op placeholder
            ;

#else
            throw new NotImplementedException( "SetScoringSystem() is not yet implemented. Need to figure out how to handle different scoring systems." );
#endif


        }
        #endregion

        #region ISaveToFile Implementation

        /// <inheritdoc/>
        public string GetFileName() {
            return $"{ProjectName}.orion";
        }

        /// <summary>
        /// SAves the current Match object to a JSON file in the specified relative directory.
        /// The file name is derived from the Match's Name property.
        /// </summary>
        /// <param name="relativeDirectory">Usually the My Matches directory.</param>
        /// <returns>The full path to the saved file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">Thrown if the MatchProject property Project is not set.</exception>
        public string SaveToFile( DirectoryInfo relativeDirectory ) {

            if (relativeDirectory == null)
                throw new ArgumentNullException( nameof( relativeDirectory ) );

            this.MatchFileName = this.Match.GetRelativePath();
            string filePath = Path.Combine( relativeDirectory.FullName, GetRelativePath() );

            var directoryPath = Path.GetDirectoryName( filePath );

            if (!Directory.Exists( directoryPath )) {
                Directory.CreateDirectory( directoryPath );
            }

            string json = G_NS.JsonConvert.SerializeObject( this, SerializerOptions.NewtonsoftJsonSerializer );

            File.WriteAllText( filePath, json );

            this.Match.SaveToFile( this.ProjectDirectory );
            foreach (var participant in Participants) {
                participant.SaveToFile( this.ProjectDirectory );
            }

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

            this.MatchFileName = this.Match.GetRelativePath();
            string json = G_NS.JsonConvert.SerializeObject( this, SerializerOptions.NewtonsoftJsonSerializer );

            File.WriteAllText( fileInfo.FullName, json );

            this.Match.SaveToFile( this.ProjectDirectory );
            foreach (var participant in Participants) {
                participant.SaveToFile( this.ProjectDirectory );
            }

            return fileInfo.FullName;
        }

        public void SaveToFile() {
            SaveToFile( this.ProjectDirectory );
        }

        /// <summary>
        /// Returns the standard relative path for this Match. It is relative to the Match Project's path.
        /// 
        /// </summary>
        /// <param name="composite">Expected to be a <see cref="MatchProject"/> instance.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if the MatchProject property Project is not set.</exception>
        public string GetRelativePath() {
            return this.GetFileName();
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

        private void RenameFile( object sender, EventArgs<MatchProject> e ) {
            /*
             * If the Project Name changes, we need to rename the file to match the new project name. 
             * This is because the file name is based on the Project Name, and we want to keep it consistent.
             */

            if (!string.IsNullOrEmpty( e.Value.ProjectName )
                && !string.IsNullOrEmpty( e.Value.PreviousProjectName )) {
                var previousFileName = $"{e.Value.PreviousProjectName}.orion";
                var newFileName = this.GetFileName();

                string oldPath = Path.Combine( e.Value.ProjectDirectory.FullName, previousFileName );
                string newPath = Path.Combine( e.Value.ProjectDirectory.FullName, newFileName );

                if (File.Exists( oldPath )) {
                    File.Move( oldPath, newPath );
                }

                this.SaveToFile();
            }
        }
        #endregion
    }
}

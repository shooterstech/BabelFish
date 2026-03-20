namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class MatchProject {

        #region Constructors, Facory Methods, and Initialization Methods
        private MatchProject( Match match, DirectoryInfo projectDirectory ) {
            if (match.Name.Length < 10)
                throw new ArgumentException( "Match name must be at least 10 characters long to ensure valid file names and paths." );

            Match = match;
            Match.Project = this;
            ProjectDirectory = projectDirectory;
            ProjectName = StringFormatting.MakeSafeFileName( match.Name );
        }

        public static async Task<MatchProject> CreateAsync( Match match, DirectoryInfo myMatchesDirectory ) {
            DirectoryInfo projectDirectory = new DirectoryInfo( Path.Combine( myMatchesDirectory.FullName, StringFormatting.MakeSafeFileName( match.Name ) ) );
            return new MatchProject( match, projectDirectory );
        }
        #endregion

        public DirectoryInfo ProjectDirectory { get; set; }

        public string ProjectName { get; private set; }

        public Match Match { get; private set; }

        public List<MatchParticipant> Participants { get; set; } = new List<MatchParticipant>();

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
                mp.CreateEntry( cof.CourseOfFireId );
            }

            foreach (var attributeConfiguration in Match.MatchStructure.SharedAttributes) {
                if (attributeConfiguration.IsForIndividuals) {
                    individual.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ) );
                }
            }

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                foreach (var attributeConfiguration in cof.Attributes) {
                    if (attributeConfiguration.IsForTeams) {
                        individual.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ) );
                    }
                }
            }

            return mp;
        }

        public async Task<MatchParticipant> CreateMatchParticipantAsync( string teamName ) {

            MatchParticipant mp = new MatchParticipant( this, ParticipantType.TEAM );
            mp.Project = this;
            mp.MatchID = Match.MatchID;
            mp.MatchName = Match.Name;

            var team = (Team)mp.Participant;
            team.TeamName = teamName;
            Participants.Add( mp );

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                mp.CreateEntry( cof.CourseOfFireId );
            }

            foreach (var attributeConfiguration in Match.MatchStructure.SharedAttributes) {
                team.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ) );
            }

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                foreach (var attributeConfiguration in cof.Attributes) {
                    team.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attributeConfiguration ) );
                }
            }
            return mp;
        }
    }
}

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

            foreach (var attrValue in Match.MatchStructure.SharedAttributes) {
                individual.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attrValue ) );
            }

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                foreach (var attrValue in cof.Attributes) {
                    individual.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attrValue ) );
                }
            }

            return mp;
        }
    }
}

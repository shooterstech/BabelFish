using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class MatchComposite : IBaseName {

        public MatchComposite( Match match ) {
            Match = match;
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            BaseName = new string( match.Name.Select( c => invalidChars.Contains( c ) ? '_' : c ).ToArray() );
        }

        public Match Match { get; private set; }

        public string BaseName { get; private set; }

        public List<MatchParticipant> Participants { get; set; } = new List<MatchParticipant>();

        public MatchParticipant CreateMatchParticipant( string familyName, string givenName ) {

            MatchParticipant mp = new MatchParticipant();
            mp.Participant = new Individual() {
                FamilyName = familyName,
                GivenName = givenName
            };

            Participants.Add( mp );

            foreach (var cof in Match.MatchStructure.CoursesOfFire) {
                mp.CreateEntry( cof.CourseOfFireId );
            }

            return mp;
        }
    }
}

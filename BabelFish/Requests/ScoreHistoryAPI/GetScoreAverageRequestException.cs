
namespace Scopos.BabelFish.Requests.ScoreHistory {
    public class GetScoreAverageRequestException : RequestException {
        public GetScoreAverageRequestException()
            : base( "Something bad happened!" ) {
        }
        public GetScoreAverageRequestException( string message )
            : base( message ) {
        }
        public GetScoreAverageRequestException( string message, Exception inner )
            : base( message, inner ) {
        }
    }
}

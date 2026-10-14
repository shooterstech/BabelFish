
namespace Scopos.BabelFish.Requests.ScoreHistory {
    public class GetScoreAverageRequestException : APIRequestParameterException {
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

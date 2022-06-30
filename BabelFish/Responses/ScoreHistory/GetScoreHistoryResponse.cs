using Newtonsoft.Json.Linq;
using ShootersTech.Responses;
using ShootersTech.DataModel.ScoreHistory;
using ShootersTech.Requests.ScoreHistoryAPI;

namespace ShootersTech.Responses.ScoreHistoryAPI {
    public class GetScoreHistoryResponse : Response<ScoreHistory>
    {

        public GetScoreHistoryResponse( GetScoreHistoryRequest request) {
            this.Request = request;
        }

        public ScoreHistory ScoreHistory
        {
            get { return Value; }
        }
    }
}

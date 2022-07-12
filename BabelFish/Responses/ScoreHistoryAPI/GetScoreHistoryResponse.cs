using Newtonsoft.Json.Linq;
using ShootersTech.BabelFish.Responses;
using ShootersTech.BabelFish.DataModel.ScoreHistory;
using ShootersTech.BabelFish.Requests.ScoreHistoryAPI;

namespace ShootersTech.BabelFish.Responses.ScoreHistoryAPI {
    public class GetScoreHistoryResponse : Response<ScoreHistoryWrapper>
    {

        public GetScoreHistoryResponse( GetScoreHistoryRequest request) {
            this.Request = request;
        }

        public ScoreHistory ScoreHistory
        {
            get { return Value.ScoreHistory; }
        }
    }
}

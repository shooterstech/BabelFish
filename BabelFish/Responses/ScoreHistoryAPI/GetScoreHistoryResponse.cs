using Newtonsoft.Json.Linq;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
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

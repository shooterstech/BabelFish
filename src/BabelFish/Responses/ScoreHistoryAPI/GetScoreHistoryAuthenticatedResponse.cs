using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
    public class GetScoreHistoryAuthenticatedResponse : GetScoreHistoryAbstractResponse {

        public GetScoreHistoryAuthenticatedResponse( GetScoreHistoryAuthenticatedRequest request) {
            this.Request = request;
        }
    }
}

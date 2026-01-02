using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
    public class GetScoreHistoryPublicResponse : GetScoreHistoryAbstractResponse {

        public GetScoreHistoryPublicResponse( GetScoreHistoryPublicRequest request) {
            this.Request = request;
        }
    }
}

using Newtonsoft.Json.Linq;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
    public class GetScoreHistoryAuthenticatedResponse : Response<ScoreHistoryWrapper>, ITokenResponse<GetScoreHistoryAuthenticatedRequest> {

        public GetScoreHistoryAuthenticatedResponse( GetScoreHistoryAuthenticatedRequest request) {
            this.Request = request;
        }

        public ScoreHistoryList ScoreHistory
        {
            get { return Value.ScoreHistoryList; }
        }

        /// <inheritdoc/>
        public GetScoreHistoryAuthenticatedRequest GetNextRequest() {
            var nextRequest = (GetScoreHistoryAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.ScoreHistoryList.NextToken;
            return nextRequest;
        }
    }
}

using Newtonsoft.Json.Linq;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {
    public class GetScoreHistoryPublicResponse : Response<ScoreHistoryWrapper>, ITokenResponse<GetScoreHistoryPublicRequest> {

        public GetScoreHistoryPublicResponse( GetScoreHistoryPublicRequest request) {
            this.Request = request;
        }

        public ScoreHistory ScoreHistory
        {
            get { return Value.ScoreHistoryList; }
        }

        /// <inheritdoc/>
        public GetScoreHistoryPublicRequest GetNextRequest() {
            var nextRequest = (GetScoreHistoryPublicRequest)Request.Copy();
            nextRequest.Token = Value.ScoreHistoryList.NextToken;
            return nextRequest;
        }
    }
}

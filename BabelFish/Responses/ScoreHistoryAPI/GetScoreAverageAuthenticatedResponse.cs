using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {

    public class GetScoreAverageAuthenticatedResponse : Response<ScoreAverageWrapper>, ITokenResponse<GetScoreAverageAuthenticatedRequest> {

        public GetScoreAverageAuthenticatedResponse( GetScoreAverageAuthenticatedRequest request) {
            this.Request = request;
        }

        public ScoreAverageList ScoreAverage {
            get { return Value.ScoreAverageList; }
        }

        /// <inheritdoc/>
        public GetScoreAverageAuthenticatedRequest GetNextRequest() {
            var nextRequest = (GetScoreAverageAuthenticatedRequest)Request.Copy();
            nextRequest.Token = Value.ScoreAverageList.NextToken;
            return nextRequest;
        }
    }
}

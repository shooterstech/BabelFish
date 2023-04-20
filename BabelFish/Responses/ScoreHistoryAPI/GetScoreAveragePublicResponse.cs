using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {

    public class GetScoreAveragePublicResponse : Response<ScoreAverageWrapper>, ITokenResponse<GetScoreAveragePublicRequest> {

        public GetScoreAveragePublicResponse( GetScoreAveragePublicRequest request) {
            this.Request = request;
        }

        public ScoreAverage ScoreAverage {
            get { return Value.ScoreAverageList; }
        }

        /// <inheritdoc/>
        public GetScoreAveragePublicRequest GetNextRequest() {
            var nextRequest = (GetScoreAveragePublicRequest)Request.Copy();
            nextRequest.Token = Value.ScoreAverageList.NextToken;
            return nextRequest;
        }
    }
}

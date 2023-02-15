using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {

    public class GetScoreAverageAuthenticatedResponse : Response<ScoreAverageWrapper> {

        public GetScoreAverageAuthenticatedResponse( GetScoreAverageAuthenticatedRequest request) {
            this.Request = request;
        }

        public ScoreAverage ScoreAverage {
            get { return Value.ScoreAverage; }
        }
    }
}

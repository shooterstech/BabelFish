using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {

    public class GetScoreAverageResponse : Response<ScoreAverageWrapper> {

        public GetScoreAverageResponse( GetScoreAverageRequest request) {
            this.Request = request;
        }

        public ScoreAverage ScoreAverage {
            get { return Value.ScoreAverage; }
        }
    }
}

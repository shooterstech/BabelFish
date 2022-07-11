using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.BabelFish.Responses;
using ShootersTech.BabelFish.DataModel.ScoreHistory;
using ShootersTech.BabelFish.Requests;
using ShootersTech.BabelFish.Requests.ScoreHistoryAPI;

namespace ShootersTech.BabelFish.Responses.ScoreHistoryAPI {

    public class GetScoreAverageResponse : Response<ScoreAverageWrapper> {

        public GetScoreAverageResponse( GetScoreAverageRequest request) {
            this.Request = request;
        }

        public ScoreAverage ScoreAverage {
            get { return Value.ScoreAverage; }
        }
    }
}

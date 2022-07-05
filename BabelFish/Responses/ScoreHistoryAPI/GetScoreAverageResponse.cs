using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.Responses;
using ShootersTech.DataModel.ScoreHistory;
using ShootersTech.Requests;
using ShootersTech.Requests.ScoreHistoryAPI;

namespace ShootersTech.Responses.ScoreHistoryAPI {

    public class GetScoreAverageResponse : Response<ScoreAverageWrapper> {

        public GetScoreAverageResponse( GetScoreAverageRequest request) {
            this.Request = request;
        }

        public ScoreAverage ScoreAverage {
            get { return Value.ScoreAverage; }
        }
    }
}

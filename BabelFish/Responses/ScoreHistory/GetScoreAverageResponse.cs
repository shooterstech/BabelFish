using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.Responses;
using ShootersTech.DataModel.ScoreHistory;

namespace ShootersTech.Responses.ScoreHistory {

    public class GetScoreAverageResponse : Response<ScoreAverage> {

        public ScoreAverage ScoreAverage {
            get { return Value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.Requests.ScoreHistoryAPI {
    public class GetScoreAverageRequest : GetScoreHistoryAbstractRequest {


        /// <inheritdoc />
        public GetScoreAverageRequest() { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/average"; }
        }

    }
}

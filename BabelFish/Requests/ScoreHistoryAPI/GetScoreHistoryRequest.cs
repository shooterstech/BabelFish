using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.Requests.ScoreHistoryAPI {
    public class GetScoreHistoryRequest : GetScoreHistoryAbstractRequest {

        /// <inheritdoc />
        public GetScoreHistoryRequest() { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/history"; }
        }
    }
}

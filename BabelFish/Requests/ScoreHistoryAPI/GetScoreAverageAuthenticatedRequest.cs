using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class GetScoreAverageAuthenticatedRequest : GetScoreHistoryAbstractRequest {


        /// <inheritdoc />
        public GetScoreAverageAuthenticatedRequest() : base( "GetScoreHistory" ) { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/average"; }
        }

    }
}

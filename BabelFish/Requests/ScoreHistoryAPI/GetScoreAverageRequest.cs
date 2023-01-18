using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class GetScoreAverageRequest : GetScoreHistoryAbstractRequest {


        /// <inheritdoc />
        public GetScoreAverageRequest() : base( "GetScoreHistory" ) { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/average"; }
        }

    }
}

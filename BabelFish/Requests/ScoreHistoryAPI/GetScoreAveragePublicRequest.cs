using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class GetScoreAveragePublicRequest : GetScoreHistoryAbstractRequest {


        /// <inheritdoc />
        public GetScoreAveragePublicRequest() : base( "GetScoreHistory" ) { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/average"; }
        }
    }
}

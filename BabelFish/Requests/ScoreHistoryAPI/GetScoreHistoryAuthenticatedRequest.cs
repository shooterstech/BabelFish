using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class GetScoreHistoryAuthenticatedRequest : GetScoreHistoryAbstractRequest {

        /// <inheritdoc />
        public GetScoreHistoryAuthenticatedRequest(UserAuthentication credentials ) : base( "GetScoreHistory", credentials ) { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/history"; }
        }
    }
}

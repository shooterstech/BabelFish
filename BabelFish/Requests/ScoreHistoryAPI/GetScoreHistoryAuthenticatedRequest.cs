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

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetScoreHistoryAuthenticatedRequest( Credentials );
            newRequest.EventStyleDef = this.EventStyleDef;
            newRequest.StageStyleDefs = this.StageStyleDefs;
            newRequest.Limit = this.Limit;
            newRequest.StartDate = this.StartDate;
            newRequest.EndDate = this.EndDate;
            newRequest.IncludeRelated = this.IncludeRelated;
            newRequest.UserIds = this.UserIds;
            newRequest.Format = this.Format;

            return newRequest;
        }
    }
}

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

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetScoreAveragePublicRequest( );
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

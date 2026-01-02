using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class GetScoreAverageAuthenticatedRequest : GetScoreHistoryAbstractRequest {


        /// <inheritdoc />
        public GetScoreAverageAuthenticatedRequest( UserAuthentication credentials ) : base( "GetScoreAverage", credentials ) { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/average"; }
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetScoreAverageAuthenticatedRequest( Credentials );
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

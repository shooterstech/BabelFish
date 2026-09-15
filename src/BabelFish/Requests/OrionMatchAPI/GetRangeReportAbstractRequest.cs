using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetRangeReportAbstractRequest : Request {

        public GetRangeReportAbstractRequest( string operationId ) : base( operationId ) {
        }

        public GetRangeReportAbstractRequest( string operationId, UserAuthentication credentials ) : base( operationId, credentials ) {
        }

        public GetRangeReportAbstractRequest( string operationId, MatchID matchId, string resultName ) : base( operationId ) {
            MatchId = matchId;
            ResultName = resultName;
        }

        public GetRangeReportAbstractRequest(
            string operationId,
            MatchID matchId,
            string resultName,
            UserAuthentication credentials ) : base( operationId, credentials ) {
            MatchId = matchId;
            ResultName = resultName;
        }

        public static GetRangeReportAbstractRequest Factory( MatchID matchId, string resultName, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return new GetRangeReportPublicRequest( matchId, resultName );
            } else {
                return new GetRangeReportAuthenticatedRequest( matchId, resultName, credentials );
            }
        }

        public MatchID? MatchId { get; set; }

        public string ResultName { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to get a range report." );
                }

                return $"/range-reporter/{MatchId}";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (string.IsNullOrWhiteSpace( ResultName )) {
                    throw new ArgumentNullException( nameof( ResultName ), "The result name must be set to get a range report." );
                }

                return new Dictionary<string, List<string>> {
                    { "result-name", new List<string> { ResultName } }
                };
            }
        }
    }
}

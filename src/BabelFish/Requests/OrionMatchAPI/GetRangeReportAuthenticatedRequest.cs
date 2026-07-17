using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetRangeReportAuthenticatedRequest : Request {

        public GetRangeReportAuthenticatedRequest( UserAuthentication credentials ) : base( "GetRangeReport", credentials ) {
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public GetRangeReportAuthenticatedRequest(
            MatchID matchId,
            string resultName,
            UserAuthentication credentials ) : this( credentials ) {
            MatchId = matchId;
            ResultName = resultName;
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

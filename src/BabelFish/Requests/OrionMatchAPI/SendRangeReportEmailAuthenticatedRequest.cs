using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class SendRangeReportEmailAuthenticatedRequest : Request {

        public SendRangeReportEmailAuthenticatedRequest( UserAuthentication credentials ) : base( "SendRangeReportEmail", credentials ) {
            HttpMethod = HttpMethod.Post;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public SendRangeReportEmailAuthenticatedRequest(
            MatchID matchId,
            IEnumerable<string> resultNames,
            UserAuthentication credentials ) : this( credentials ) {
            MatchId = matchId;
            ResultNames = resultNames?.ToList() ?? throw new ArgumentNullException( nameof( resultNames ) );
        }

        public MatchID? MatchId { get; set; }

        public List<string> ResultNames { get; set; } = new List<string>();

        /// <summary>
        /// If true, the API builds the email and reports its recipients without sending it.
        /// </summary>
        public bool DryRun { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to send a range report email." );
                }

                return $"/range-reporter/{MatchId}/email";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (ResultNames == null || ResultNames.Count == 0) {
                    throw new ArgumentException( "At least one result name must be set to send a range report email.", nameof( ResultNames ) );
                }

                if (ResultNames.Any( string.IsNullOrWhiteSpace )) {
                    throw new ArgumentException( "Result names must be non-empty strings.", nameof( ResultNames ) );
                }

                var parameterList = new Dictionary<string, List<string>> {
                    { "result-name", ResultNames }
                };

                if (DryRun) {
                    parameterList.Add( "dry-run", new List<string> { DryRun.ToString() } );
                }

                return parameterList;
            }
        }
    }
}

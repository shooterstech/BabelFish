using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PatchRangeReportAuthenticatedRequest : Request {

        public PatchRangeReportAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchRangeReport", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public PatchRangeReportAuthenticatedRequest(
            MatchID matchId,
            string resultName,
            UserAuthentication credentials ) : this( credentials ) {
            MatchId = matchId;
            ResultName = resultName;
        }

        public MatchID? MatchId { get; set; }

        public string ResultName { get; set; } = string.Empty;

        public bool? Published { get; set; }

        public string? FormattedHtml { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to patch a range report." );
                }

                return $"/range-reporter/{MatchId}";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (string.IsNullOrWhiteSpace( ResultName )) {
                    throw new ArgumentNullException( nameof( ResultName ), "The result name must be set to patch a range report." );
                }

                var parameterList = new Dictionary<string, List<string>> {
                    { "result-name", new List<string> { ResultName } }
                };

                if (Published.HasValue) {
                    parameterList.Add( "Published", new List<string> { Published.Value.ToString() } );
                }

                if (FormattedHtml != null) {
                    parameterList.Add( "FormattedHtml", new List<string> { FormattedHtml } );
                }

                if (!Published.HasValue && FormattedHtml == null) {
                    throw new ArgumentException( "At least one of published or formatted HTML must be provided." );
                }

                return parameterList;
            }
        }
    }
}

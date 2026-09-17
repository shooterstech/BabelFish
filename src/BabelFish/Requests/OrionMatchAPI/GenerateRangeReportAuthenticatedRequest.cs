using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GenerateRangeReportAuthenticatedRequest : RangeReportGenerationAuthenticatedRequest {

        public GenerateRangeReportAuthenticatedRequest( UserAuthentication credentials ) : base( "GenerateRangeReport", credentials ) {
        }

        public GenerateRangeReportAuthenticatedRequest(
            MatchID matchId,
            string resultListName,
            UserAuthentication credentials ) : this( credentials ) {
            MatchId = matchId;
            ResultListName = resultListName;
        }

        public MatchID? MatchId { get; set; }

        public string ResultListName { get; set; } = string.Empty;

        [Obsolete( "The RangeReporter API ignores milestone-strategy. Use MilestoneCount or ShotMilestoneCounts instead." )]
        public string? MilestoneStrategy { get; set; }

        [Obsolete( "ExpectedShots is derived by the RangeReporter service and cannot be supplied as a generation option." )]
        public int? ExpectedShots { get; set; }

        [Obsolete( "The RangeReporter API ignores snapshot-order-by." )]
        public string? SnapshotOrderBy { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to generate a range report." );
                }

                return $"/range-reporter/{MatchId}";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (string.IsNullOrWhiteSpace( ResultListName )) {
                    throw new ArgumentNullException( nameof( ResultListName ), "The result list name must be set to generate a range report." );
                }

                Dictionary<string, List<string>> parameterList = BuildGenerationQueryParameters();
                parameterList.Add( "result-name", new List<string> { ResultListName } );

                return parameterList;
            }
        }
    }
}
